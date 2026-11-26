import { createHash, randomUUID } from 'node:crypto';
import { readFile } from 'node:fs/promises';
import { resolve } from 'node:path';
import { fileURLToPath, pathToFileURL } from 'node:url';

const datasetPath = new URL('./blog-posts.json', import.meta.url);
const guidPattern = /^[0-9a-f]{8}-(?:[0-9a-f]{4}-){3}[0-9a-f]{12}$/i;

export function sampleKey(parentId, id) {
    const bytes = createHash('sha256')
        .update(`umbracoblog-sample-post:${parentId.toLowerCase()}:${id}`)
        .digest().subarray(0, 16);
    bytes.subarray(0, 4).reverse();
    bytes.subarray(4, 6).reverse();
    bytes.subarray(6, 8).reverse();
    const hex = bytes.toString('hex');
    return `${hex.slice(0, 8)}-${hex.slice(8, 12)}-${hex.slice(12, 16)}-${hex.slice(16, 20)}-${hex.slice(20)}`;
}

export function validatePosts(posts) {
    if(!Array.isArray(posts) || posts.length === 0) {
        throw new Error('The sample dataset is empty.');
    }

    const ids = new Set();
    for(const post of posts) {
        if(!post || typeof post.id !== 'string' || !post.id.trim() || ids.has(post.id)
            || typeof post.title !== 'string' || !post.title.trim()
            || !/^\d{4}-\d{2}-\d{2}$/.test(post.date)
            || Number.isNaN(Date.parse(`${post.date}T00:00:00Z`))
            || new Date(`${post.date}T00:00:00Z`).toISOString().slice(0, 10) !== post.date
            || !Array.isArray(post.tags) || post.tags.length === 0
            || post.tags.some(tag => typeof tag !== 'string' || !tag.trim())
            || !Array.isArray(post.paragraphs) || post.paragraphs.length === 0
            || post.paragraphs.some(paragraph => typeof paragraph !== 'string' || !paragraph.trim())
            || (post.code && !['csharp', 'javascript'].includes(post.codeLanguage))) {
            throw new Error(`Invalid sample post: ${post?.id ?? '(missing id)'}.`);
        }

        ids.add(post.id);
    }
}

function localOrigin(baseUrl) {
    const url = new URL(baseUrl);
    if(url.protocol !== 'https:'
        || !['localhost', '127.0.0.1', '[::1]'].includes(url.hostname)
        || url.username || url.password || url.pathname !== '/' || url.search || url.hash) {
        throw new Error('The site URL must be a localhost HTTPS origin with no path or credentials.');
    }

    return url.origin;
}

function property(alias, value) {
    return { alias, value, culture: null, segment: null };
}

function createBlocks(post, contentTypeId, codeTypeId) {
    const escape = text => text.replaceAll('&', '&amp;').replaceAll('<', '&lt;').replaceAll('>', '&gt;').replaceAll('"', '&quot;');
    const content = [{
        contentTypeKey: contentTypeId,
        key: randomUUID(),
        values: [property('content', {
            markup: post.paragraphs.map(paragraph => `<p>${escape(paragraph)}</p>`).join(''),
            blocks: { layout: {}, contentData: [], settingsData: [], expose: [] }
        })]
    }];

    if(post.code) {
        content.push({
            contentTypeKey: codeTypeId,
            key: randomUUID(),
            values: [property('code', post.code), property('language', [post.codeLanguage])]
        });
    }

    return {
        layout: { 'Umbraco.BlockList': content.map(item => ({ contentKey: item.key, settingsKey: null })) },
        contentData: content,
        settingsData: [],
        expose: content.map(item => ({ contentKey: item.key, culture: null, segment: null }))
    };
}

export async function importSamplePosts({ baseUrl, parentId, token, cookie, posts, fetchImpl = fetch, log = console.log }) {
    const origin = localOrigin(baseUrl);
    if(!guidPattern.test(parentId) || !token || !cookie || /[\r\n]/.test(token + cookie)) {
        throw new Error('A Blog Index GUID and the authenticated Management API token and session cookie are required.');
    }

    validatePosts(posts);
    parentId = parentId.toLowerCase();
    const api = `${origin}/umbraco/management/api/v1`;
    const headers = { authorization: `Bearer ${token}`, cookie, 'content-type': 'application/json' };

    async function request(path, options = {}) {
        const response = await fetchImpl(`${api}${path}`, { ...options, headers, redirect: 'error' });
        if(!response.ok) {
            throw new Error(`Management API ${options.method ?? 'GET'} ${path} failed with HTTP ${response.status}.`);
        }

        return response.status === 204 ? null : response;
    }

    async function get(path) {
        return (await request(path)).json();
    }

    async function allPages(path) {
        const items = [];
        let total;
        do {
            const separator = path.includes('?') ? '&' : '?';
            const page = await get(`${path}${separator}skip=${items.length}&take=100`);
            total = page.total;
            if(!Array.isArray(page.items) || (page.items.length === 0 && items.length < total)) {
                throw new Error(`Incomplete Management API listing: ${path}.`);
            }

            items.push(...page.items);
        } while(items.length < total);
        return items;
    }

    async function findType(items, alias) {
        const types = await Promise.all(items.map(item => get(`/document-type/${item.id}`)));
        const matches = types.filter(type => type.alias === alias);
        if(matches.length !== 1) {
            throw new Error(`Expected exactly one document type with alias '${alias}'.`);
        }

        return matches[0];
    }

    const parent = await get(`/document/${parentId}`);
    const parentType = await get(`/document-type/${parent.documentType.id}`);
    if(parentType.alias !== 'blogIndex' || !parent.variants.some(variant => variant.state === 'Published')) {
        throw new Error('The target must be a published Blog Index.');
    }

    const allowed = await allPages(`/document-type/${parent.documentType.id}/allowed-children`);
    const postType = await findType(allowed, 'blogPost');
    const library = await allPages('/document-type/allowed-in-library');
    const [contentType, codeType] = await Promise.all([
        findType(library, 'contentBlock'),
        findType(library, 'codeBlock')
    ]);
    if(!postType.defaultTemplate?.id) {
        throw new Error('The Blog Post document type must have a default template.');
    }

    const children = await allPages(`/tree/document/children?parentId=${parentId}`);
    const childIds = new Set(children.map(child => child.id.toLowerCase()));
    const pending = [];
    for(const post of posts) {
        const id = sampleKey(parentId, post.id);
        const response = await fetchImpl(`${api}/document/${id}`, { headers, redirect: 'error' });
        if(response.status === 404) {
            pending.push({ post, id });
            continue;
        }

        if(!response.ok) {
            throw new Error(`Could not check sample '${post.id}': HTTP ${response.status}.`);
        }

        const existing = await response.json();
        if(!childIds.has(id) || existing.documentType.id !== postType.id
            || !existing.variants.some(variant => variant.state === 'Published')) {
            throw new Error(`Sample '${post.id}' conflicts with existing content or is a draft; no changes were made.`);
        }
    }

    log(`Target: ${origin}, Blog Index ${parentId}; ${pending.length} to create, ${posts.length - pending.length} already present.`);
    for(const { post, id } of pending) {
        const body = {
            id,
            parent: { id: parentId },
            documentType: { id: postType.id },
            template: { id: postType.defaultTemplate.id },
            variants: [{ name: post.title, culture: null, segment: null }],
            values: [
                property('mainHeading', post.title),
                property('publishDate', `${post.date} 12:00:00`),
                property('tags', post.tags),
                property('mainContent', createBlocks(post, contentType.id, codeType.id))
            ]
        };
        try {
            await request('/document', { method: 'POST', body: JSON.stringify(body) });
        } catch(error) {
            throw new Error(`Could not create '${post.id}'; check for a draft before retrying. ${error.message}`);
        }
        try {
            await request(`/document/${id}/publish`, {
                method: 'PUT',
                body: JSON.stringify({ publishSchedules: [{ culture: null, schedule: null }] })
            });
        } catch(error) {
            throw new Error(`Could not publish '${post.id}'; a draft may exist and must be inspected before retrying. ${error.message}`);
        }

        log(`Published: ${post.title}`);
    }

    return { created: pending.length, skipped: posts.length - pending.length };
}

async function main() {
    const [, , baseUrl, parentId] = process.argv;
    if(process.argv.length !== 4) {
        throw new Error('Usage: node scripts\\import-sample-posts.mjs <localhost-site-url> <blog-index-guid>');
    }

    const posts = JSON.parse(await readFile(fileURLToPath(datasetPath), 'utf8'));
    const result = await importSamplePosts({
        baseUrl,
        parentId,
        token: process.env.UMBRACO_SAMPLE_TOKEN,
        cookie: process.env.UMBRACO_SAMPLE_COOKIE,
        posts
    });
    console.log(`Complete: ${result.created} created, ${result.skipped} unchanged.`);
}

if(process.argv[1] && pathToFileURL(resolve(process.argv[1])).href === import.meta.url) {
    main().catch(error => {
        console.error(error.message);
        process.exitCode = 1;
    });
}

import assert from 'node:assert/strict';
import { readFile } from 'node:fs/promises';
import test from 'node:test';
import { importSamplePosts, sampleKey, validatePosts } from './import-sample-posts.mjs';

const parentId = 'eb02ea7d-be85-4454-8b4b-92363616b269';
const post = {
    id: 'example',
    title: 'Searching code examples',
    date: '2025-01-17',
    tags: ['search', 'JavaScript'],
    paragraphs: ['A useful explanation.', 'Another paragraph with <markup>.'],
    code: 'console.log("hello");',
    codeLanguage: 'javascript'
};

function fakeApi() {
    const records = new Map();
    const mutations = [];
    let failPublish = false;
    const types = {
        index: { id: 'index', alias: 'blogIndex' },
        post: { id: 'post', alias: 'blogPost', defaultTemplate: { id: 'template' } },
        content: { id: 'content', alias: 'contentBlock' },
        code: { id: 'code', alias: 'codeBlock' }
    };
    const response = (status, data) => ({ status, ok: status >= 200 && status < 300, json: async () => data });
    const fetchImpl = async (url, options) => {
        const path = new URL(url).pathname.replace('/umbraco/management/api/v1', '');
        if(path === `/document/${parentId}`) {
            return response(200, { documentType: { id: 'index' }, variants: [{ state: 'Published' }] });
        }

        if(path === '/document-type/index/allowed-children') {
            return response(200, { total: 1, items: [{ id: 'post' }] });
        }

        if(path === '/document-type/allowed-in-library') {
            return response(200, { total: 2, items: [{ id: 'content' }, { id: 'code' }] });
        }

        if(path.startsWith('/document-type/')) {
            return response(200, types[path.split('/').at(-1)]);
        }

        if(path === '/tree/document/children') {
            const items = [...records.keys()].map(id => ({ id }));
            return response(200, { total: items.length, items });
        }

        if(path === '/document' && options.method === 'POST') {
            const body = JSON.parse(options.body);
            mutations.push(body);
            records.set(body.id, { documentType: { id: 'post' }, variants: [{ state: 'Draft' }] });
            return response(201);
        }

        const publish = path.match(/^\/document\/([^/]+)\/publish$/);
        if(publish) {
            if(failPublish) {
                return response(400);
            }

            records.get(publish[1]).variants[0].state = 'Published';
            return response(200);
        }

        const document = path.match(/^\/document\/([^/]+)$/);
        if(document) {
            const found = records.get(document[1]);
            return response(found ? 200 : 404, found);
        }

        throw new Error(`Unexpected request: ${path}`);
    };

    return { records, mutations, fetchImpl, failPublishing: () => { failPublish = true; } };
}

function run(api, posts = [post]) {
    return importSamplePosts({
        baseUrl: 'https://localhost:44363',
        parentId,
        token: 'test-token',
        cookie: 'test-cookie',
        posts,
        fetchImpl: api.fetchImpl,
        log: () => {}
    });
}

test('dataset has unique complete posts and related topic clusters', async () => {
    const posts = JSON.parse(await readFile(new URL('./blog-posts.json', import.meta.url), 'utf8'));
    validatePosts(posts);
    assert.ok(posts.length >= 60);
    assert.ok(posts.filter(item => item.tags.includes('search')).length > 10);
    assert.ok(posts.some(item => item.code));
    assert.throws(() => validatePosts([post, post]));
    assert.throws(() => validatePosts([{ ...post, date: '2025-02-31' }]));
});

test('sample keys are stable and parent scoped', () => {
    assert.equal(sampleKey(parentId, 'async-cancellation'), 'd5e286a5-0329-bf9f-853e-10dc6f4f2551');
    assert.notEqual(sampleKey(parentId, post.id), sampleKey('00000000-0000-0000-0000-000000000001', post.id));
});

test('creates and publishes content, then leaves it unchanged on a repeat run', async () => {
    const api = fakeApi();
    assert.deepEqual(await run(api), { created: 1, skipped: 0 });
    const body = api.mutations[0];
    assert.equal(body.parent.id, parentId);
    assert.deepEqual(body.values.find(item => item.alias === 'tags').value, post.tags);
    const blocks = body.values.find(item => item.alias === 'mainContent').value.contentData;
    assert.match(blocks[0].values[0].value.markup, /&lt;markup&gt;/);
    assert.deepEqual(blocks[1].values.find(item => item.alias === 'language').value, ['javascript']);
    assert.deepEqual(await run(api), { created: 0, skipped: 1 });
    assert.equal(api.mutations.length, 1);
});

test('detects existing drafts before creating any missing post', async () => {
    const api = fakeApi();
    const other = { ...post, id: 'other' };
    api.records.set(sampleKey(parentId, other.id), { documentType: { id: 'post' }, variants: [{ state: 'Draft' }] });
    await assert.rejects(run(api, [post, other]), /conflicts with existing content or is a draft/);
    assert.equal(api.mutations.length, 0);
});

test('reports publish failure and refuses to overwrite the resulting draft', async () => {
    const api = fakeApi();
    api.failPublishing();
    await assert.rejects(run(api), /draft may exist/);
    await assert.rejects(run(api), /conflicts with existing content or is a draft/);
    assert.equal(api.mutations.length, 1);
});

test('refuses remote sites without sending credentials', async () => {
    let called = false;
    await assert.rejects(importSamplePosts({
        baseUrl: 'https://example.com', parentId, token: 'token', cookie: 'cookie', posts: [post],
        fetchImpl: async () => { called = true; }
    }), /localhost HTTPS/);
    assert.equal(called, false);
});

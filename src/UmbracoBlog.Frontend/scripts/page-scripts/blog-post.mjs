export default (rootElement = document.body) => {
  const blogPost = rootElement.querySelector('.blog-post');

  if(!blogPost) {
    return;
  }

  const tocContainer = rootElement.querySelector('.table-of-contents');
  if(tocContainer) {
    const toc = generateTableOfContents(blogPost);
    tocContainer.appendChild(toc);
  }

  addHeadingAnchors(blogPost);

  // Since we added heading anchors late, we need to scroll to the target
  // element if there's a hash in the URL.
  if(!!window.location.hash) {
    const targetId = window.location.hash.substring(1);
    const targetElement = document.getElementById(targetId);

    if(!!targetElement) {
      targetElement.scrollIntoView();
    }
  }
};

/**
 * Adds anchor links to all headings (h2, h3, h4) within the given blog post element.
 * @param {HTMLElement} blogPost - The blog post element containing the headings.
 */
function addHeadingAnchors(blogPost) {
  const headings = blogPost.querySelectorAll('h2, h3, h4');

  headings.forEach(heading => {
    const id = getHeadingId(heading);
    heading.id = id;

    const anchor = document.createElement('a');
    anchor.href = `#${id}`;
    anchor.textContent = '#';
    anchor.className = 'heading-anchor';

    heading.appendChild(anchor);
  });
}

/**
 * Generates a table of contents for the given blog post element.
 * @param {HTMLElement} blogPost - The blog post element containing the headings.
 * @returns {HTMLElement} The generated table of contents as an unordered list.
 */
function generateTableOfContents(blogPost) {
  const headings = blogPost.querySelectorAll('h2, h3, h4');
  const toc = document.createElement('ol');

  headings.forEach(heading => {
    const id = getHeadingId(heading);
    heading.id = id;

    const li = document.createElement('li');
    const a = document.createElement('a');
    a.href = `#${id}`;
    a.textContent = heading.textContent;
    li.appendChild(a);
    toc.appendChild(li);
  });

  return toc;
}

/**
 * Gets the ID for a heading element, generating one if it doesn't exist.
 * @param {HTMLElement} heading - The heading element.
 * @returns {string} The ID of the heading.
 */
function getHeadingId(heading) {
  return heading.id || heading.textContent.toLowerCase().replace(/\s+/g, '-').replace(/[^\w-]/g, '');
}


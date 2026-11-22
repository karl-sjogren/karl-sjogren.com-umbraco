import { createHighlighter } from 'shiki';
import {
  transformerNotationFocus,
  transformerRenderLineNumber,
} from '@shikijs/transformers';

const highlighter = await createHighlighter({
  themes: ['github-dark-default'],
  langs: ['javascript', 'csharp', 'shell', 'bash', 'plain']
});

export default (rootElement = document.body) => {
  const codeBlocks = rootElement.querySelectorAll('pre code');

  codeBlocks.forEach((block) => {
    const language = block.dataset.lang || 'plain';
    const html = highlighter.codeToHtml(block.textContent, {
      lang: language,
      theme: 'github-dark-default',
      transformers: [
        transformerNotationFocus(),
        transformerRenderLineNumber()
      ]
    });

    const htmlFragment = document.createRange().createContextualFragment(html);
    block.parentElement.replaceWith(htmlFragment);
  });
};

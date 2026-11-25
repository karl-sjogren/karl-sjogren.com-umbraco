import { createHighlighter } from 'shiki';
import {
  transformerNotationFocus,
  transformerRenderLineNumber,
} from '@shikijs/transformers';

const highlighter = await createHighlighter({
  themes: ['github-dark-default'],
  langs: ['javascript', 'csharp', 'shell', 'bash', 'plain', 'xml', 'css', 'sass', 'json', 'html', 'razor']
});

export default (rootElement = document.body) => {
  const codeBlocks = rootElement.querySelectorAll('.code-block-container');

  codeBlocks.forEach(container => {
    const block = container.querySelector('code');

    const language = block.dataset.lang || 'plain';
    const code = block.textContent;

    const html = highlighter.codeToHtml(code, {
      lang: language,
      theme: 'github-dark-default',
      transformers: [
        transformerNotationFocus(),
        transformerRenderLineNumber()
      ]
    });

    const htmlFragment = document.createRange().createContextualFragment(html);
    block.parentElement.replaceWith(htmlFragment);

    const button = container.querySelector('.code-block-copy-code-button');
    if(button) {
      button.addEventListener('click', () => {
        navigator.clipboard.writeText(code);
      });
    }
  });
};

import mermaid from 'mermaid';

mermaid.initialize({ theme: 'default' });

export default (rootElement = document.body) => {
  const mermaidBlocks = rootElement.querySelectorAll('.mermaid-block-container');

  mermaidBlocks.forEach(container => {
    const content = container.querySelector('.mermaid-block-content');

    const script = content.textContent;

    mermaid.render(
      '',
      script
    ).then(({ svg }) => {
      content.innerHTML = svg;
    });

    const button = container.querySelector('.code-block-copy-code-button');
    if(button) {
      button.addEventListener('click', () => {
        navigator.clipboard.writeText(script);
      });
    }
  });
};

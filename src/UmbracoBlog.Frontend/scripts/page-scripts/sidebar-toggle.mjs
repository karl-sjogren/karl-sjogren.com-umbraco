export default (rootElement = document.body) => {
  const sidebarToggleButtons = rootElement.querySelectorAll('.sidebar-toggle, .sidebar-close');
  const sidebar = rootElement.querySelector('aside');

  if(sidebarToggleButtons && sidebar) {
    sidebarToggleButtons.forEach(button => {
      button.addEventListener('click', () => {
        sidebar.classList.toggle('open');
      });
    });
  }
};

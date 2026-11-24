export default (rootElement = document.body) => {
  const sidebarToggleButtons = rootElement.querySelectorAll('.sidebar-toggle, .sidebar-close');
  const sidebar = rootElement.querySelector('.sidebar');

  if(sidebarToggleButtons && sidebar) {
    sidebarToggleButtons.forEach(button => {
      button.addEventListener('click', () => {
        sidebar.classList.toggle('open');
      });
    });
  }
};

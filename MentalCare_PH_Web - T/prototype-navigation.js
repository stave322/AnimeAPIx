(function () {
  const root = window.location.pathname.replace(/\\/g, '/');
  const isAdmin = root.includes('/Admin_WEB/');
  const isClient = root.includes('/Client_WEB/');
  const isCounselor = root.includes('/Counselor_WEB/');

  const pageMap = isAdmin
    ? {
        dashboard: '../Admin_Dashboard/index_1.html',
        users: '../Admin_UserManage/index_2.html',
        content: '../Admin_Content/index_3.html',
        reports: '../Admin_Reports/index_4.html',
        roles: '../Admin_Role/index_5.html',
        settings: '../Admin_Settings/index_6.html',
        audit: '../Admin_Audit/index_7.html'
      }
    : isClient
      ? {
          home: '../Client_Dashboard/index_2.html',
          resources: '../Client_Resources/index_3.html',
          appointment: '../Client_Appointment/index_6.html',
          assessment: '../Client_Assessment/index_4.html',
          notifications: '../Client_Notifications/index_5.html',
          profile: '../Client_Profile/index_7.html',
          virtual: '../Client_Virtual/index_8.html'
        }
      : isCounselor
        ? {
            dashboard: '../Counselor_Dashboard/index_2.html',
            schedule: '../Counselor_Schedule/index_3.html',
            clients: '../Counselor_Review/index_4.html',
            sessions: '../Counselor_Virtual/index_5.html',
            notifications: '../Counselor_Notifications/index_6.html',
            profile: '../Counselor_Profile/index_7.html'
          }
        : {};

  function goToPage(page) {
    if (page === 'logout') {
      localStorage.removeItem('mc_session');
      sessionStorage.removeItem('mc_session');
      window.location.href = isCounselor
        ? '../Counselor_Login/index.html'
        : isClient
          ? '../Client_Login/index.html'
          : '../Admin_login/index.html';
      return;
    }

    if (pageMap[page]) {
      window.location.href = pageMap[page];
    }
  }

  function addPrototypeLogout() {
    if (document.querySelector('[data-page="logout"]')) return;

    if (isAdmin) {
      const sidebarNav = document.querySelector('.sidebar-nav');
      if (!sidebarNav) return;

      const logout = document.createElement('button');
      logout.className = 'nav-item';
      logout.dataset.page = 'logout';
      logout.type = 'button';
      logout.innerHTML = '<span class="nav-icon">🚪</span> Log Out';
      sidebarNav.appendChild(logout);
      return;
    }

    if (isClient) {
      const navRight = document.querySelector('.nav-right');
      if (!navRight) return;

      const logout = document.createElement('button');
      logout.className = 'prototype-logout';
      logout.dataset.page = 'logout';
      logout.type = 'button';
      logout.textContent = 'Log Out';
      navRight.appendChild(logout);

      const style = document.createElement('style');
      style.textContent = '.prototype-logout{border:1px solid #d8e1e5;border-radius:6px;background:#fff;color:#0e3a5c;padding:7px 11px;font:500 12px Poppins,Segoe UI,sans-serif;cursor:pointer}.prototype-logout:hover{background:#e6f6f1}';
      document.head.appendChild(style);
    }
  }

  const shortcutRoutes = {
    bellBtn: 'notifications',
    avatarBtn: 'profile',
    joinBtn: 'virtual'
  };

  document.addEventListener('click', function (event) {
    const button = event.target.closest('button[id]');
    if (!button || !isClient || !shortcutRoutes[button.id]) return;

    const page = shortcutRoutes[button.id];
    const currentPage = pageMap[page] && pageMap[page].split('/').pop();
    if (currentPage && window.location.pathname.endsWith(currentPage)) return;

    event.preventDefault();
    event.stopImmediatePropagation();
    goToPage(page);
  }, true);

  document.addEventListener('click', function (event) {
    const target = event.target.closest('[data-page]');
    if (!target || (target.dataset.page !== 'logout' && !pageMap[target.dataset.page])) return;

    event.preventDefault();
    goToPage(target.dataset.page);
  });

  document.querySelectorAll('button').forEach(function (button) {
    button.addEventListener('keydown', function (event) {
      if (event.key === 'Enter' || event.key === ' ') {
        button.classList.add('prototype-focused');
      }
    });
  });

  addPrototypeLogout();
})();

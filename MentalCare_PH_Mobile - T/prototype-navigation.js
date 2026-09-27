(function () {
  const isCounselor = window.location.pathname.includes('Counselor_MOBILE');
  const isRoleScreen = window.location.pathname.includes('Client_Role');
  const clientPrefix = isRoleScreen ? '../Client_MOBILE/' : '../';
  const routes = isCounselor
    ? {
        home: '../Counselor_Home/index_2.html',
        schedule: '../Counselor_Schedule/index_3.html',
        clients: '../Counselor_Client/index_4.html',
        alerts: '../Counselor_Notification/index_5.html',
        profile: '../Counselor_Profile/index_6.html',
        login: '../Counselor_Login/index.html',
        create: '../Counselor_CreateAcc/index_1.html',
        virtual: '../Counselor_Virtual/index_7.html'
      }
    : {
        home: `${clientPrefix}Client_Home/index_3.html`,
        assess: `${clientPrefix}Client_Assessment/index_4.html`,
        appts: `${clientPrefix}Client_Appointments/index_5.html`,
        learn: `${clientPrefix}Client_Resources/index_7.html`,
        profile: `${clientPrefix}Client_Profile/index_8.html`,
        login: `${clientPrefix}Client_Login/index.html`,
        create: `${clientPrefix}Client_CreateAcc/index_2.html`,
        book: `${clientPrefix}Client_BookAppoint/index_6.html`,
        virtual: `${clientPrefix}Client_Virtual/index_9.html`
      };

  function navigate(path) {
    if (path) window.location.href = path;
  }

  document.addEventListener('click', function (event) {
    const target = event.target.closest('button, a, [role="button"]');
    if (!target) return;

    const tab = target.closest('.tab-item');
    if (tab && routes[tab.dataset.tab]) {
      event.preventDefault();
      navigate(routes[tab.dataset.tab]);
      return;
    }

    const actionRoutes = {
      clientCard: isCounselor ? null : routes.login,
      counselorCard: isCounselor ? null : '../Counselor_MOBILE/Counselor_Login/index.html',
      backRoleBtn: '../../Client_Role/index_1.html',
      backLoginBtn: routes.login,
      loginLink: routes.login,
      signupLink: routes.create,
      requestLink: routes.create,
      logoutBtn: routes.login,
      avatarBtn: routes.profile,
      joinBtn: isCounselor ? routes.virtual : routes.virtual,
      assessCard: isCounselor ? null : routes.assess,
      foryouItem: isCounselor ? null : routes.learn,
      bookBtn: isCounselor ? null : routes.book,
      rescheduleBtn: isCounselor ? null : routes.book,
      startBtn: isCounselor ? routes.virtual : null,
      startSessionBtn: isCounselor ? routes.virtual : null,
      notesBtn: isCounselor ? routes.clients : null,
      endBtn: routes.home
    };

    if (Object.prototype.hasOwnProperty.call(actionRoutes, target.id) && actionRoutes[target.id]) {
      event.preventDefault();
      navigate(actionRoutes[target.id]);
    }
  });
})();

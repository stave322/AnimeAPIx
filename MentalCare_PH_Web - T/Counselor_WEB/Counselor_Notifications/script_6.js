const notifications = [
  {
    icon: '📅',
    title: 'New appointment request',
    desc: 'R. Bautista requested a session on Fri, Aug 16, 1:00 PM.',
    time: '20 min ago',
    unread: true
  },
  {
    icon: '✏️',
    title: 'Client rescheduled',
    desc: 'K. Villanueva moved their session to 2:00 PM.',
    time: '1 hour ago',
    unread: true
  },
  {
    icon: '✅',
    title: 'Session completed',
    desc: 'Your session with J. Dela Cruz was marked complete.',
    time: 'Yesterday',
    unread: false
  }
];

const notifList = document.getElementById('notifList');
const markAllBtn = document.getElementById('markAllBtn');

function renderNotifications() {
  notifList.innerHTML = '';

  if (notifications.length === 0) {
    const empty = document.createElement('div');
    empty.className = 'empty-state';
    empty.textContent = "You're all caught up. No notifications yet.";
    notifList.appendChild(empty);
    return;
  }

  notifications.forEach((n, i) => {
    const card = document.createElement('div');
    card.className = 'notif-card' + (n.unread ? ' unread' : '');
    card.innerHTML = `
      <div class="notif-icon">${n.icon}</div>
      <div class="notif-body">
        <div class="notif-title">${n.title}</div>
        <p class="notif-desc">${n.desc}</p>
        <span class="notif-time">${n.time}</span>
      </div>
    `;
    card.addEventListener('click', () => {
      notifications[i].unread = false;
      renderNotifications();
    });
    notifList.appendChild(card);
  });
}

markAllBtn.addEventListener('click', () => {
  notifications.forEach(n => n.unread = false);
  renderNotifications();
});

document.querySelectorAll('.nav-item').forEach(item => {
  item.addEventListener('click', function () {
    document.querySelectorAll('.nav-item').forEach(i => i.classList.remove('active'));
    this.classList.add('active');
  });
});

renderNotifications();
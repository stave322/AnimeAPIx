const notifications = [
  {
    icon: '📅',
    title: 'Appointment confirmed',
    desc: 'Your session with Ms. A. Reyes on Aug 14, 2:00 PM is confirmed.',
    time: '2 hours ago',
    unread: true
  },
  {
    icon: '⏰',
    title: 'Reminder',
    desc: 'Your appointment starts in 24 hours. Tap to view details.',
    time: '6 hours ago',
    unread: true
  },
  {
    icon: '📚',
    title: 'New resource added',
    desc: '"Managing exam stress" was just published to your resource library.',
    time: 'Yesterday',
    unread: false
  },
  {
    icon: '⏱️',
    title: 'Continue your check-in',
    desc: 'You have 4 questions left on your Stress & Anxiety Check-in.',
    time: '2 days ago',
    unread: false
  }
];

const notifList = document.getElementById('notifList');
const markAllBtn = document.getElementById('markAllBtn');
const bellDot = document.getElementById('bellDot');

function updateBellDot() {
  const hasUnread = notifications.some(n => n.unread);
  bellDot.classList.toggle('hidden', !hasUnread);
}

function renderNotifications() {
  notifList.innerHTML = '';

  if (notifications.length === 0) {
    const empty = document.createElement('div');
    empty.className = 'empty-state';
    empty.textContent = "You're all caught up. No notifications yet.";
    notifList.appendChild(empty);
    updateBellDot();
    return;
  }

  notifications.forEach((n, i) => {
    const card = document.createElement('div');
    card.className = 'notif-card' + (n.unread ? ' unread' : '');
    card.innerHTML = `
      <div class="notif-icon">${n.icon}</div>
      <div class="notif-body">
        <div class="notif-title-row">
          <span class="notif-title">${n.title}</span>
          ${n.unread ? '<span class="unread-dot"></span>' : ''}
        </div>
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

  updateBellDot();
}

markAllBtn.addEventListener('click', () => {
  notifications.forEach(n => n.unread = false);
  renderNotifications();
});

document.getElementById('bellBtn').addEventListener('click', () => {
  document.getElementById('notifList').scrollIntoView({ behavior: 'smooth' });
});

document.querySelectorAll('.nav-links a').forEach(link => {
  link.addEventListener('click', function (e) {
    e.preventDefault();
    document.querySelectorAll('.nav-links a').forEach(l => l.classList.remove('active'));
    this.classList.add('active');
  });
});

document.getElementById('avatarBtn').addEventListener('click', () => {
  alert('Profile menu goes here.');
});

renderNotifications();
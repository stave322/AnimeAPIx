const requests = [
  { name: 'R.Bautista', meta: 'Fri, Aug 16 · 1:00 PM' },
  { name: 'D. Mendoza', meta: 'Mon, Aug 19 · 9:00 AM' },
  { name: 'K. Villanueva', meta: 'Tue, Aug 20 · 3:00 PM' }
];

const requestList = document.getElementById('requestList');
const pendingCount = document.getElementById('pendingCount');

function updatePendingCount() {
  const remaining = requests.filter(r => !r.resolved).length;
  pendingCount.textContent = `${remaining} request${remaining === 1 ? '' : 's'}`;
}

function renderRequests() {
  requestList.innerHTML = '';

  if (requests.length === 0) {
    const empty = document.createElement('div');
    empty.className = 'empty-note';
    empty.textContent = 'No pending requests.';
    requestList.appendChild(empty);
    return;
  }

  requests.forEach((r, i) => {
    const card = document.createElement('div');
    card.className = 'request-card' + (r.resolved ? ' resolved' : '');

    if (r.resolved) {
      card.innerHTML = `
        <div class="request-name">${r.name}</div>
        <div class="request-meta">${r.meta}</div>
        <div class="request-status-label">${r.resolved === 'accepted' ? 'Accepted' : 'Declined'}</div>
      `;
    } else {
      card.innerHTML = `
        <div class="request-name">${r.name}</div>
        <div class="request-meta">${r.meta}</div>
        <div class="request-actions">
          <button class="btn-decline">Decline</button>
          <button class="btn-accept">Accept</button>
        </div>
      `;
      card.querySelector('.btn-decline').addEventListener('click', () => {
        requests[i].resolved = 'declined';
        updatePendingCount();
        renderRequests();
      });
      card.querySelector('.btn-accept').addEventListener('click', () => {
        requests[i].resolved = 'accepted';
        updatePendingCount();
        renderRequests();
      });
    }

    requestList.appendChild(card);
  });
}

document.getElementById('startBtn').addEventListener('click', function () {
  this.textContent = 'Session in progress';
  this.disabled = true;
});

document.getElementById('avatarBtn').addEventListener('click', () => {
  alert('Profile menu goes here.');
});

document.querySelectorAll('.tab-item').forEach(tab => {
  tab.addEventListener('click', function () {
    document.querySelectorAll('.tab-item').forEach(t => t.classList.remove('active'));
    this.classList.add('active');
  });
});

renderRequests();
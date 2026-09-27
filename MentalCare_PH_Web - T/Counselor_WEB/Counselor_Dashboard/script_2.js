const schedule = [
  { time: '9:00 AM', client: 'J.Dela Cruz', type: 'Virtual', status: 'confirmed' },
  { time: '10:30 AM', client: 'M. Santos', type: 'In-person', status: 'confirmed' },
  { time: '1:00 PM', client: 'R. Bautista', type: 'Virtual', status: 'pending' },
  { time: '2:00 PM', client: 'B. Ryan', type: 'Virtual', status: 'confirmed' }
];

const scheduleBody = document.getElementById('scheduleBody');

function renderSchedule() {
  scheduleBody.innerHTML = '';
  schedule.forEach(s => {
    const row = document.createElement('tr');
    const isPending = s.status === 'pending';
    row.innerHTML = `
      <td>${s.time}</td>
      <td>${s.client}</td>
      <td>${s.type}</td>
      <td><span class="status-pill ${s.status}">${isPending ? 'Pending' : 'Confirmed'}</span></td>
      <td><button class="view-btn ${isPending ? 'respond' : ''}">${isPending ? 'Respond' : 'View'}</button></td>
    `;
    row.querySelector('.view-btn').addEventListener('click', () => {
      alert(isPending ? `Responding to ${s.client}'s request` : `Viewing session with ${s.client}`);
    });
    scheduleBody.appendChild(row);
  });
}

const requests = [
  { name: 'R. Bautista', meta: 'Fri, Aug 16 · 1:00 PM' },
  { name: 'D. Mendoza', meta: 'Mon, Aug 19 · 9:00 AM' }
];

const requestList = document.getElementById('requestList');
const pendingCount = document.getElementById('pendingCount');

function renderRequests() {
  requestList.innerHTML = '';
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

function updatePendingCount() {
  const remaining = requests.filter(r => !r.resolved).length + 1; // +1 for the pending schedule item
  pendingCount.textContent = remaining;
}

document.getElementById('blockTimeBtn').addEventListener('click', () => {
  alert('Opening "Block Time" scheduler...');
});

document.querySelectorAll('.nav-item').forEach(item => {
  item.addEventListener('click', function () {
    document.querySelectorAll('.nav-item').forEach(i => i.classList.remove('active'));
    this.classList.add('active');
  });
});

renderSchedule();
renderRequests();
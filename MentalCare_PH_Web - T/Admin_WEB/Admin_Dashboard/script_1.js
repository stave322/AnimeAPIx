const activity = [
  { action: 'Account created', user: 'J. Torres, RGC', table: 'Counselors', time: '10 min ago' },
  { action: 'Content published', user: 'System Admin', table: 'Educational Content', time: '1 hr ago' },
  { action: 'Role updated', user: 'System Admin', table: 'Users', time: '3 hrs ago' },
  { action: 'Account suspended', user: 'System Admin', table: 'Users', time: 'Yesterday' }
];

const activityBody = document.getElementById('activityBody');

function renderActivity() {
  activityBody.innerHTML = '';
  activity.forEach(a => {
    const row = document.createElement('tr');
    row.innerHTML = `
      <td>${a.action}</td>
      <td>${a.user}</td>
      <td>${a.table}</td>
      <td>${a.time}</td>
    `;
    activityBody.appendChild(row);
  });
}

const requests = [
  { name: 'M. Cruz, RPsy', meta: 'Counselor account request', type: 'counselor' },
  { name: '"Managing exam stress"', meta: 'New article for review', type: 'content' }
];

const requestList = document.getElementById('requestList');
const pendingVerification = document.getElementById('pendingVerification');
const draftsPending = document.getElementById('draftsPending');

function updateCounts() {
  const pendingCounselors = requests.filter(r => r.type === 'counselor' && !r.resolved).length;
  const pendingContent = requests.filter(r => r.type === 'content' && !r.resolved).length;
  pendingVerification.textContent = `${pendingCounselors} pending verification`;
  draftsPending.textContent = `${pendingContent} drafts pending`;
}

function renderRequests() {
  requestList.innerHTML = '';
  requests.forEach((r, i) => {
    const card = document.createElement('div');
    card.className = 'request-card' + (r.resolved ? ' resolved' : '');

    const approveLabel = r.type === 'content' ? 'Published' : 'Approve';
    const rejectLabel = 'Reject';

    if (r.resolved) {
      card.innerHTML = `
        <div class="request-name">${r.name}</div>
        <div class="request-meta">${r.meta}</div>
        <div class="request-status-label">${r.resolved === 'approved' ? (r.type === 'content' ? 'Published' : 'Approved') : 'Rejected'}</div>
      `;
    } else {
      card.innerHTML = `
        <div class="request-name">${r.name}</div>
        <div class="request-meta">${r.meta}</div>
        <div class="request-actions">
          <button class="btn-reject">${rejectLabel}</button>
          <button class="btn-approve">${approveLabel}</button>
        </div>
      `;
      card.querySelector('.btn-reject').addEventListener('click', () => {
        requests[i].resolved = 'rejected';
        updateCounts();
        renderRequests();
      });
      card.querySelector('.btn-approve').addEventListener('click', () => {
        requests[i].resolved = 'approved';
        updateCounts();
        renderRequests();
      });
    }

    requestList.appendChild(card);
  });
}

document.getElementById('reportBtn').addEventListener('click', () => {
  alert('Generating system-wide report...');
});

document.querySelectorAll('.nav-item').forEach(item => {
  item.addEventListener('click', function () {
    document.querySelectorAll('.nav-item').forEach(i => i.classList.remove('active'));
    this.classList.add('active');
  });
});

renderActivity();
renderRequests();
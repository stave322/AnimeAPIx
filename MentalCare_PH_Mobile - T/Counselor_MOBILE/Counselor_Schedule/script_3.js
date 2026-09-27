const scheduleByDay = {
  'Wed 14': [
    { time: '9:00 AM', client: 'Juana Dela Cruz', type: 'Virtual', status: 'confirmed' },
    { time: '10:30 AM', client: 'M. Santos', type: 'In-person', status: 'confirmed' },
    { time: '1:00 PM', client: 'R. Bautista', type: 'Virtual', status: 'pending' }
  ],
  'Thu 15': [
    { time: '11:00 AM', client: 'K. Villanueva', type: 'Virtual', status: 'confirmed' }
  ],
  'Fri 16': [
    { time: '1:00 PM', client: 'R. Bautista', type: 'Virtual', status: 'pending' }
  ],
  'Sat 17': []
};

const apptList = document.getElementById('apptList');
const dayChips = document.querySelectorAll('.day-chip');
let activeDay = 'Wed 14';

function renderSchedule() {
  apptList.innerHTML = '';
  const items = scheduleByDay[activeDay] || [];

  if (items.length === 0) {
    const empty = document.createElement('div');
    empty.className = 'empty-state';
    empty.textContent = 'No sessions scheduled for this day.';
    apptList.appendChild(empty);
    return;
  }

  items.forEach(item => {
    const card = document.createElement('div');
    card.className = 'appt-card';
    card.innerHTML = `
      <div class="appt-top-row">
        <span class="appt-time">${item.time}</span>
        <span class="status-pill ${item.status}">${item.status === 'pending' ? 'Pending' : 'Confirmed'}</span>
      </div>
      <div class="appt-meta">${item.client} · ${item.type}</div>
    `;
    card.addEventListener('click', () => {
      alert(`${item.client} — ${activeDay}, ${item.time} (${item.type})`);
    });
    apptList.appendChild(card);
  });
}

dayChips.forEach(chip => {
  chip.addEventListener('click', function () {
    dayChips.forEach(c => c.classList.remove('active'));
    this.classList.add('active');
    activeDay = this.dataset.day;
    renderSchedule();
  });
});

document.getElementById('calendarBtn').addEventListener('click', () => {
  alert('Opening full calendar view...');
});

document.querySelectorAll('.tab-item').forEach(tab => {
  tab.addEventListener('click', function () {
    document.querySelectorAll('.tab-item').forEach(t => t.classList.remove('active'));
    this.classList.add('active');
  });
});

renderSchedule();
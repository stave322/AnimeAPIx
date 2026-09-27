const weeks = [
  {
    label: 'Week of Aug 12 – 16',
    days: ['Mon 12', 'Tue 13', 'Wed 14', 'Thu 15', 'Fri 16'],
    times: ['9:00 AM', '11:00 AM', '2:00 PM'],
    events: {
      '9:00 AM': { 'Tue 13': { name: 'J. Dela Cruz', type: 'confirmed-green' } },
      '11:00 AM': {
        'Mon 12': { name: 'M. Santos', type: 'confirmed-blue' },
        'Thu 15': { name: 'K. Villanueva', type: 'confirmed-green' }
      },
      '2:00 PM': {
        'Wed 14': { name: 'K. Villanueva', type: 'confirmed-green' },
        'Fri 16': { name: 'Pending', type: 'pending' }
      }
    }
  },
  {
    label: 'Week of Aug 19 – 23',
    days: ['Mon 19', 'Tue 20', 'Wed 21', 'Thu 22', 'Fri 23'],
    times: ['9:00 AM', '11:00 AM', '2:00 PM'],
    events: {
      '9:00 AM': { 'Mon 19': { name: 'D. Mendoza', type: 'pending' } },
      '11:00 AM': { 'Wed 21': { name: 'J. Dela Cruz', type: 'confirmed-green' } },
      '2:00 PM': { 'Fri 23': { name: 'M. Santos', type: 'confirmed-blue' } }
    }
  },
  {
    label: 'Week of Aug 5 – 9',
    days: ['Mon 5', 'Tue 6', 'Wed 7', 'Thu 8', 'Fri 9'],
    times: ['9:00 AM', '11:00 AM', '2:00 PM'],
    events: {
      '9:00 AM': { 'Thu 8': { name: 'K. Villanueva', type: 'confirmed-green' } },
      '11:00 AM': { 'Tue 6': { name: 'M. Santos', type: 'confirmed-blue' } },
      '2:00 PM': {}
    }
  }
];

let currentWeekIndex = 0; // start on "Week of Aug 12-16"
const weekLabel = document.getElementById('weekLabel');
const calendarGrid = document.getElementById('calendarGrid');

function renderWeek() {
  const week = weeks[currentWeekIndex];
  weekLabel.textContent = week.label;
  calendarGrid.innerHTML = '';

  // header row
  calendarGrid.appendChild(document.createElement('div')); // empty corner
  week.days.forEach(day => {
    const dh = document.createElement('div');
    dh.className = 'day-header';
    dh.textContent = day;
    calendarGrid.appendChild(dh);
  });

  // time rows
  week.times.forEach(time => {
    const tl = document.createElement('div');
    tl.className = 'time-label';
    tl.textContent = time;
    calendarGrid.appendChild(tl);

    week.days.forEach(day => {
      const cell = document.createElement('div');
      cell.className = 'day-cell';
      const event = week.events[time] && week.events[time][day];
      if (event) {
        const pill = document.createElement('div');
        pill.className = `event-pill ${event.type}`;
        pill.textContent = event.name;
        pill.addEventListener('click', () => {
          alert(`${event.name} — ${day}, ${time}`);
        });
        cell.appendChild(pill);
      }
      calendarGrid.appendChild(cell);
    });
  });
}

document.getElementById('prevBtn').addEventListener('click', () => {
  currentWeekIndex = (currentWeekIndex + 1) % weeks.length === 2 ? 2 : (currentWeekIndex === 0 ? 2 : currentWeekIndex - 1);
  renderWeek();
});

document.getElementById('nextBtn').addEventListener('click', () => {
  currentWeekIndex = currentWeekIndex === 2 ? 0 : (currentWeekIndex + 1) % weeks.length;
  renderWeek();
});

document.getElementById('availBtn').addEventListener('click', () => {
  alert('Opening availability settings...');
});

document.querySelectorAll('.nav-item').forEach(item => {
  item.addEventListener('click', function () {
    document.querySelectorAll('.nav-item').forEach(i => i.classList.remove('active'));
    this.classList.add('active');
  });
});

renderWeek();
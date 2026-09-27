const STORAGE_KEY = 'mentalcare_confirmed_appointments';
const RESCHEDULE_KEY = 'mentalcare_reschedule_target';

const apptList = document.getElementById('apptList');
const chips = document.querySelectorAll('.filter-chip');
let activeFilter = 'upcoming';

function safeRead(key, fallback) {
  try {
    const stored = localStorage.getItem(key);
    return stored ? JSON.parse(stored) : fallback;
  } catch (error) {
    return fallback;
  }
}

function safeWrite(key, value) {
  try {
    localStorage.setItem(key, JSON.stringify(value));
  } catch (error) {
    console.warn('Unable to save local data.', error);
  }
}

function formatDateKey(date) {
  const year = date.getFullYear();
  const month = String(date.getMonth() + 1).padStart(2, '0');
  const day = String(date.getDate()).padStart(2, '0');
  return `${year}-${month}-${day}`;
}

function toDateTime(dateISO, timeText) {
  const [timePart, meridiem] = timeText.trim().split(' ');
  const [hourStr, minuteStr] = timePart.split(':');
  let hour = Number(hourStr);

  if (meridiem === 'PM' && hour < 12) {
    hour += 12;
  }

  if (meridiem === 'AM' && hour === 12) {
    hour = 0;
  }

  const baseDate = new Date(`${dateISO}T00:00:00`);
  baseDate.setHours(hour, Number(minuteStr || 0), 0, 0);
  return baseDate;
}

function getAppointments() {
  const stored = safeRead(STORAGE_KEY, []);
  let appointments = Array.isArray(stored)
    ? stored.filter(appointment => appointment && appointment.id !== 'sample-appointment')
    : [];

  return appointments.sort((a, b) => (b.createdAt || 0) - (a.createdAt || 0));
}

function getAppointmentCategory(appointment) {
  const appointmentDate = toDateTime(appointment.dateISO, appointment.time);
  const now = new Date();

  if (appointmentDate < now) {
    return 'past';
  }

  return 'upcoming';
}

function isJoinWindow(appointment) {
  const now = new Date();
  const startTime = toDateTime(appointment.dateISO, appointment.time);
  const isToday = appointment.dateISO === formatDateKey(now);
  const joinWindowStart = new Date(startTime.getTime() - 10 * 60 * 1000);
  const joinWindowEnd = new Date(startTime.getTime() - 5 * 60 * 1000);

  return isToday && now >= joinWindowStart && now <= joinWindowEnd;
}

function renderAppointments() {
  apptList.innerHTML = '';

  const filtered = getAppointments().filter(appointment => {
    const category = getAppointmentCategory(appointment);
    return activeFilter === category;
  });

  if (filtered.length === 0) {
    const empty = document.createElement('div');
    empty.className = 'empty-state';
    empty.textContent = `No ${activeFilter} appointments.`;
    apptList.appendChild(empty);
    return;
  }

  filtered.forEach(appointment => {
    const card = document.createElement('div');
    card.className = 'appt-card';

    const category = getAppointmentCategory(appointment);
    const statusLabel = category === 'past' ? 'Completed' : 'Confirmed';
    const cancelDisabled = category !== 'upcoming';

    card.innerHTML = `
      <div class="appt-top-row">
        <span class="status-badge ${category === 'past' ? 'confirmed' : 'confirmed'}">${statusLabel}</span>
        <span class="mode-label">${appointment.sessionType === 'virtual' ? 'Virtual' : appointment.sessionType}</span>
      </div>
      <div class="appt-mid-row">
        <div class="appt-avatar" style="background:#1aab8a">${appointment.counselorInitials || 'PL'}</div>
        <div>
          <div class="appt-name">${appointment.counselorName}</div>
          <div class="appt-meta">${appointment.dateISO} · ${appointment.time}</div>
        </div>
      </div>
      <div class="appt-actions">
        <button class="btn-reschedule" ${category === 'past' ? 'disabled' : ''}>Reschedule</button>
        <button class="btn-cancel" ${cancelDisabled ? 'disabled' : ''}>Cancel</button>
      </div>
    `;

    const rescheduleBtn = card.querySelector('.btn-reschedule');
    const cancelBtn = card.querySelector('.btn-cancel');

    if (rescheduleBtn && category !== 'past') {
      rescheduleBtn.addEventListener('click', () => {
        safeWrite(RESCHEDULE_KEY, { id: appointment.id });
        window.location.href = '../Client_BookAppoint/index_6.html';
      });
    }

    if (cancelBtn && !cancelDisabled) {
      cancelBtn.addEventListener('click', () => {
        const remaining = getAppointments().filter(item => item.id !== appointment.id);
        safeWrite(STORAGE_KEY, remaining);
        renderAppointments();
      });
    }

    apptList.appendChild(card);
  });
}

chips.forEach(chip => {
  chip.addEventListener('click', function () {
    chips.forEach(c => c.classList.remove('active'));
    this.classList.add('active');
    activeFilter = this.dataset.filter;
    renderAppointments();
  });
});

document.getElementById('bookBtn').addEventListener('click', () => {
  localStorage.removeItem(RESCHEDULE_KEY);
  window.location.href = '../Client_BookAppoint/index_6.html';
});

document.querySelectorAll('.tab-item').forEach(tab => {
  tab.addEventListener('click', function () {
    document.querySelectorAll('.tab-item').forEach(t => t.classList.remove('active'));
    this.classList.add('active');
  });
});

renderAppointments();
const STORAGE_KEY = 'mentalcare_confirmed_appointments';
const RESCHEDULE_KEY = 'mentalcare_reschedule_target';

const counselors = [
  { initials: 'PL', name: 'B. Plaza, John, RPsy', specialty: 'Anxiety, Stress', color: '#1aab8a' },
  { initials: 'JT', name: 'J. Torres, RGC', specialty: 'Academic Family', color: '#0e3a5c' },
  { initials: 'MC', name: 'M. Cruz, RPsy', specialty: 'Depression, Grief', color: '#1aab8a' }
];

const timeSlotsByDay = [
  ['9:00 AM', '10:00 AM', '2:00 PM', '3:00 PM'],
  ['9:00 AM', '11:00 AM', '1:00 PM'],
  ['10:00 AM', '2:00 PM', '4:00 PM'],
  [],
  ['9:00 AM', '3:00 PM']
];

let dateIndex = 0;
let selectedCounselor = 0;
let selectedTime = null;
let dateWindow = [];
let rescheduleTarget = null;
let rescheduleMode = false;

const counselorList = document.getElementById('counselorList');
const dateLabel = document.getElementById('dateLabel');
const timeSlotsContainer = document.getElementById('timeSlots');
const confirmBtn = document.getElementById('confirmBtn');
const confirmMsg = document.getElementById('confirmMsg');
const appointmentNoticeModal = document.getElementById('appointmentNoticeModal');
const appointmentNoticeTitle = document.getElementById('appointmentNoticeTitle');
const appointmentNoticeMessage = document.getElementById('appointmentNoticeMessage');
const closeAppointmentNoticeBtn = document.getElementById('closeAppointmentNotice');
const datePicker = document.getElementById('datePicker');
const datePickerModal = document.getElementById('datePickerModal');
const datePickerModalInput = document.getElementById('datePickerModalInput');
const calendarBtn = document.getElementById('calendarBtn');
const closeDatePickerBtn = document.getElementById('closeDatePicker');
const applyDatePickerBtn = document.getElementById('applyDatePicker');

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
    console.warn('Unable to save appointment.', error);
  }
}

function formatDateKey(date) {
  const year = date.getFullYear();
  const month = String(date.getMonth() + 1).padStart(2, '0');
  const day = String(date.getDate()).padStart(2, '0');
  return `${year}-${month}-${day}`;
}

function showNoticeModal(message, isError = false) {
  appointmentNoticeTitle.textContent = isError ? 'Appointment error' : 'Appointment successful';
  appointmentNoticeMessage.textContent = message;
  appointmentNoticeModal.classList.remove('hidden');
  appointmentNoticeModal.setAttribute('aria-hidden', 'false');
  appointmentNoticeModal.dataset.type = isError ? 'error' : 'success';
}

function closeNoticeModal() {
  appointmentNoticeModal.classList.add('hidden');
  appointmentNoticeModal.setAttribute('aria-hidden', 'true');
}

function showConfirmMessage(message, isError = false) {
  confirmMsg.textContent = message;
  confirmMsg.style.color = isError ? '#d64545' : '#1aab8a';
  showNoticeModal(message, isError);
}

function isCurrentDateBlocked(dateKey) {
  const todayKey = formatDateKey(new Date());
  return dateKey <= todayKey;
}

function loadAppointments() {
  const stored = safeRead(STORAGE_KEY, []);
  let appointments = Array.isArray(stored)
    ? stored.filter(appointment => appointment && appointment.id !== 'sample-appointment')
    : [];

  return [...appointments].sort((a, b) => (b.createdAt || 0) - (a.createdAt || 0));
}

function limitAppointments(appointments) {
  return [...appointments]
    .sort((a, b) => (b.createdAt || 0) - (a.createdAt || 0))
    .slice(0, 2);
}

function loadRescheduleTarget() {
  const stored = safeRead(RESCHEDULE_KEY, null);

  if (!stored) {
    return null;
  }

  const appointments = loadAppointments();
  return appointments.find(appointment => String(appointment.id) === String(stored.id)) || null;
}

function findCounselorIndexByAppointment(appointment) {
  if (!appointment) {
    return 0;
  }

  const targetName = (appointment.counselorName || '').toLowerCase();
  const targetInitials = (appointment.counselorInitials || '').toLowerCase();

  const found = counselors.findIndex(counselor => {
    const counselorName = (counselor.name || '').toLowerCase();
    const counselorInitials = (counselor.initials || '').toLowerCase();

    return counselorName === targetName
      || counselorInitials === targetInitials
      || counselorName.includes(targetName)
      || targetName.includes(counselorName);
  });

  return found >= 0 ? found : 0;
}

function buildDateWindow() {
  const today = new Date();
  today.setHours(0, 0, 0, 0);

  return Array.from({ length: 5 }, (_, index) => {
    const currentDate = new Date(today);
    currentDate.setDate(today.getDate() + index + 1);

    return {
      label: currentDate.toLocaleDateString('en-US', {
        weekday: 'long',
        month: 'long',
        day: 'numeric'
      }),
      iso: formatDateKey(currentDate),
      slots: timeSlotsByDay[index] || []
    };
  });
}

function updateDatePicker() {
  if (!dateWindow.length) {
    datePicker.disabled = true;
    datePicker.value = '';
    return;
  }

  datePicker.disabled = false;
  datePicker.min = dateWindow[0].iso;
  datePicker.max = dateWindow[dateWindow.length - 1].iso;
  datePicker.value = dateWindow[dateIndex].iso;
}

function renderCounselors() {
  counselorList.innerHTML = '';
  counselors.forEach((c, i) => {
    const item = document.createElement('div');
    item.className = 'counselor-item' + (i === selectedCounselor ? ' selected' : '');
    item.innerHTML = `
      <div class="counselor-avatar" style="background:${c.color}">${c.initials}</div>
      <div>
        <div class="counselor-name">${c.name}</div>
        <div class="counselor-specialty">${c.specialty}</div>
      </div>
    `;
    item.addEventListener('click', () => {
      selectedCounselor = i;
      renderCounselors();
    });
    counselorList.appendChild(item);
  });
}

function renderTimeSlots() {
  if (!dateWindow.length) {
    dateLabel.textContent = 'No earlier date available';
    timeSlotsContainer.innerHTML = '';
    const none = document.createElement('span');
    none.className = 'empty-slot-message';
    none.textContent = 'There is no earlier date available for rescheduling this appointment.';
    timeSlotsContainer.appendChild(none);
    selectedTime = null;
    updateConfirmState();
    return;
  }

  const selectedDate = dateWindow[dateIndex];
  dateLabel.textContent = selectedDate.label;
  const slots = selectedDate.slots || [];
  timeSlotsContainer.innerHTML = '';

  if (slots.length === 0) {
    const none = document.createElement('span');
    none.className = 'empty-slot-message';
    none.textContent = 'No available slots for this day.';
    timeSlotsContainer.appendChild(none);
    selectedTime = null;
  } else {
    slots.forEach(time => {
      const btn = document.createElement('button');
      btn.className = 'time-slot' + (time === selectedTime ? ' selected' : '');
      btn.textContent = time;
      btn.addEventListener('click', () => {
        selectedTime = time;
        renderTimeSlots();
        updateConfirmState();
      });
      timeSlotsContainer.appendChild(btn);
    });
  }

  updateConfirmState();
}

function updateConfirmState() {
  confirmBtn.disabled = !selectedTime || !dateWindow.length;
  confirmBtn.textContent = rescheduleMode ? 'Update appointment' : 'Confirm appointment';
}

function persistAppointment(appointment) {
  const current = loadAppointments();

  if (rescheduleMode && rescheduleTarget) {
    const existing = current.filter(item => item.id !== rescheduleTarget.id);
    const updatedAppointments = limitAppointments([appointment, ...existing]);
    safeWrite(STORAGE_KEY, updatedAppointments);
    localStorage.removeItem(RESCHEDULE_KEY);
    return;
  }

  const existing = current.filter(item => item.id !== appointment.id);
  const updatedAppointments = limitAppointments([appointment, ...existing]);
  safeWrite(STORAGE_KEY, updatedAppointments);
}

function handleConfirm() {
  if (!selectedTime || !dateWindow.length) {
    return;
  }

  const isDashboardReschedule = Boolean(rescheduleMode && rescheduleTarget);

  if (!isDashboardReschedule) {
    const existingAppointments = loadAppointments();

    if (existingAppointments.length >= 2) {
      showConfirmMessage('Appointment limit reached. You can only book up to 2 appointments at a time. Please reschedule or cancel an existing appointment first.', true);
      return;
    }
  }

  const counselor = counselors[selectedCounselor];
  const sessionType = document.getElementById('sessionType').value;
  const selectedDate = dateWindow[dateIndex];

  const appointment = {
    id: rescheduleMode && rescheduleTarget ? rescheduleTarget.id : Date.now(),
    counselorName: counselor.name,
    counselorInitials: counselor.initials,
    specialty: counselor.specialty,
    sessionType,
    dateISO: selectedDate.iso,
    time: selectedTime,
    createdAt: rescheduleMode && rescheduleTarget ? rescheduleTarget.createdAt : Date.now()
  };

  persistAppointment(appointment);

  const successMessage = rescheduleMode
    ? `Appointment rescheduled successfully for ${selectedDate.label} at ${selectedTime}.`
    : `Appointment confirmed successfully for ${selectedDate.label} at ${selectedTime}.`;

  showConfirmMessage(successMessage, false);

  confirmBtn.disabled = true;
  confirmBtn.textContent = rescheduleMode ? 'Updated' : 'Confirmed';
}

function openDatePickerModal() {
  if (datePicker.disabled) {
    return;
  }

  const startDate = dateWindow[0]?.iso || '';
  const endDate = dateWindow[dateWindow.length - 1]?.iso || '';

  datePickerModalInput.min = startDate;
  datePickerModalInput.max = endDate;
  datePickerModalInput.value = datePicker.value || startDate;
  datePickerModal.classList.add('show');
  datePickerModal.setAttribute('aria-hidden', 'false');
}

function closeDatePickerModal() {
  datePickerModal.classList.remove('show');
  datePickerModal.setAttribute('aria-hidden', 'true');
}

function applyDatePickerModal() {
  if (!datePickerModalInput.value) {
    closeDatePickerModal();
    return;
  }

  const selectedKey = datePickerModalInput.value;
  const minimumAllowedDate = datePicker.min || dateWindow[0]?.iso || '';
  const maximumAllowedDate = datePicker.max || dateWindow[dateWindow.length - 1]?.iso || '';

  if (isCurrentDateBlocked(selectedKey)) {
    showConfirmMessage("Can't Pick Current Date", true);
    closeDatePickerModal();
    return;
  }

  if ((minimumAllowedDate && selectedKey < minimumAllowedDate) || (maximumAllowedDate && selectedKey > maximumAllowedDate)) {
    showConfirmMessage('Selected date is outside the available appointment window.', true);
    closeDatePickerModal();
    return;
  }

  datePicker.value = selectedKey;
  datePicker.dispatchEvent(new Event('change'));
  closeDatePickerModal();
}

calendarBtn.addEventListener('click', openDatePickerModal);
closeDatePickerBtn.addEventListener('click', closeDatePickerModal);
applyDatePickerBtn.addEventListener('click', applyDatePickerModal);
closeAppointmentNoticeBtn.addEventListener('click', closeNoticeModal);

appointmentNoticeModal.addEventListener('click', (event) => {
  if (event.target === appointmentNoticeModal) {
    closeNoticeModal();
  }
});

datePickerModal.addEventListener('click', (event) => {
  if (event.target === datePickerModal) {
    closeDatePickerModal();
  }
});

datePicker.addEventListener('change', () => {
  if (!datePicker.value) {
    return;
  }

  if (isCurrentDateBlocked(datePicker.value)) {
    showConfirmMessage("Can't Pick Current Date", true);
    datePicker.value = dateWindow[0]?.iso || '';
    dateIndex = dateWindow.findIndex(entry => entry.iso === datePicker.value);
    dateIndex = dateIndex >= 0 ? dateIndex : 0;
    selectedTime = null;
    renderTimeSlots();
    return;
  }

  const foundIndex = dateWindow.findIndex(entry => entry.iso === datePicker.value);

  if (foundIndex >= 0) {
    dateIndex = foundIndex;
    selectedTime = null;
    renderTimeSlots();
  }
});

confirmBtn.addEventListener('click', handleConfirm);

document.querySelectorAll('.nav-links a').forEach(link => {
  link.addEventListener('click', function (event) {
    event.preventDefault();
    document.querySelectorAll('.nav-links a').forEach(item => item.classList.remove('active'));
    this.classList.add('active');
  });
});

document.getElementById('bellBtn').addEventListener('click', () => {
  alert('You have new notifications.');
});

document.getElementById('avatarBtn').addEventListener('click', () => {
  alert('Profile menu goes here.');
});

rescheduleTarget = loadRescheduleTarget();
rescheduleMode = !!rescheduleTarget;

if (rescheduleTarget) {
  selectedCounselor = findCounselorIndexByAppointment(rescheduleTarget);
}

dateWindow = buildDateWindow();
renderCounselors();
updateDatePicker();
renderTimeSlots();
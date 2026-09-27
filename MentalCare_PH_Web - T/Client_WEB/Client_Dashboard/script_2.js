const STORAGE_KEYS = {
  appointments: 'mentalcare_confirmed_appointments',
  moods: 'mentalcare_mood_log',
  assessment: 'mentalcare_daily_assessment'
};

const DASHBOARD_FOCUS_KEY = 'mentalcare_dashboard_focus_id';

const moodOptions = [
  { label: 'Great', emoji: '😄', tone: 'great' },
  { label: 'Good', emoji: '🙂', tone: 'good' },
  { label: 'Okay', emoji: '😐', tone: 'ok' },
  { label: 'Low', emoji: '🙁', tone: 'low' }
];

const recommendedResources = [
  {
    icon: '📄',
    title: '5 grounding techniques for stress',
    description: 'Quick, science-backed ways to feel calmer in the moment.',
    tags: ['stress', 'anxiety', 'low'],
    href: 'https://www.healthline.com/health/grounding-techniques'
  },
  {
    icon: '🐻',
    title: 'Guided breathing exercise',
    description: 'A short breathing reset you can do before class, work, or sleep.',
    tags: ['stress', 'anxiety', 'good', 'okay'],
    href: 'https://www.youtube.com/watch?v=O29e4rRMrV4'
  },
  {
    icon: '📄',
    title: 'How to build a self-care routine',
    description: 'Simple habits that support sleep, focus, and overall wellbeing.',
    tags: ['good', 'great', 'self-care', 'energy'],
    href: 'https://www.nimh.nih.gov/health/topics/caring-for-your-mental-health'
  },
  {
    icon: '🎵',
    title: 'Sleep wind-down meditation',
    description: 'A relaxing audio routine to help your mind settle at night.',
    tags: ['sleep', 'low', 'stress'],
    href: 'https://www.youtube.com/watch?v=eSmwk0_PXYs'
  }
];

let moodTargetDateKey = null;
let appointmentIndex = 0;

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

function getStartOfDay(date) {
  const clone = new Date(date);
  clone.setHours(0, 0, 0, 0);
  return clone;
}

function formatDisplayDate(date) {
  return date.toLocaleDateString('en-US', {
    month: 'short',
    day: 'numeric',
    year: 'numeric'
  });
}

function getWeekDates() {
  const today = getStartOfDay(new Date());
  const day = today.getDay();
  const offset = day === 0 ? -6 : 1 - day;
  const monday = new Date(today);
  monday.setDate(today.getDate() + offset);

  return Array.from({ length: 7 }, (_, index) => {
    const current = new Date(monday);
    current.setDate(monday.getDate() + index);
    return current;
  });
}

function pruneMoodData() {
  const moods = safeRead(STORAGE_KEYS.moods, []);
  const weekDates = getWeekDates();
  const currentWeekKeys = new Set(weekDates.map(date => formatDateKey(date)));

  const filtered = moods.filter(entry => currentWeekKeys.has(entry.dateKey));

  if (filtered.length !== moods.length) {
    safeWrite(STORAGE_KEYS.moods, filtered);
  }
}

function getMoodEntry(dateKey) {
  const moods = safeRead(STORAGE_KEYS.moods, []);
  return moods.find(entry => entry.dateKey === dateKey) || null;
}

function getTodayMood() {
  const todayKey = formatDateKey(new Date());
  return getMoodEntry(todayKey);
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

function ensureSeedAppointment() {
  const appointments = safeRead(STORAGE_KEYS.appointments, []);

  if (appointments.length > 0) {
    return;
  }

  const future = new Date();
  future.setDate(future.getDate() + 6);
  future.setHours(14, 0, 0, 0);

  const sampleAppointment = {
    id: 'sample-appointment',
    counselorName: 'Mr. John Carlo Plaza',
    counselorInitials: 'PL',
    specialty: 'Virtual',
    sessionType: 'virtual',
    dateISO: formatDateKey(future),
    time: '2:00 PM',
    createdAt: Date.now()
  };

  safeWrite(STORAGE_KEYS.appointments, [sampleAppointment]);
}

function getAppointments() {
  ensureSeedAppointment();
  const stored = safeRead(STORAGE_KEYS.appointments, []);
  let appointments = Array.isArray(stored)
    ? stored.filter(appointment => appointment && appointment.id !== 'sample-appointment')
    : [];

  return [...appointments]
    .sort((a, b) => (b.createdAt || 0) - (a.createdAt || 0))
    .slice(0, 2);
}

function getAppointmentsByDate() {
  return [...getAppointments()].sort((a, b) => {
    const timeA = toDateTime(a.dateISO, a.time).getTime();
    const timeB = toDateTime(b.dateISO, b.time).getTime();
    return timeA - timeB;
  });
}

function getAssessmentState() {
  const todayKey = formatDateKey(new Date());
  const state = safeRead(STORAGE_KEYS.assessment, null);

  if (state && state.dateKey === todayKey) {
    return state;
  }

  return null;
}

function getUpcomingAppointment() {
  const appointments = [...getAppointments()].sort((a, b) => {
    const timeA = toDateTime(a.dateISO, a.time).getTime();
    const timeB = toDateTime(b.dateISO, b.time).getTime();
    return timeA - timeB;
  });

  const now = new Date();
  const upcoming = appointments.find(appointment => {
    const startAt = toDateTime(appointment.dateISO, appointment.time);
    return startAt >= now;
  });

  return upcoming || appointments[appointments.length - 1] || null;
}

function renderGreeting() {
  const greetingEl = document.getElementById('greeting');
  const hour = new Date().getHours();
  let timeGreeting = 'Good morning';

  if (hour >= 12 && hour < 18) {
    timeGreeting = 'Good afternoon';
  } else if (hour >= 18) {
    timeGreeting = 'Good evening';
  }

  greetingEl.textContent = `${timeGreeting}, Ryan`;
}

function renderMoodCards() {
  const statValue = document.querySelector('.stat-card .stat-value');
  const todayMood = getTodayMood();

  if (statValue) {
    statValue.textContent = todayMood ? `${todayMood.emoji} ${todayMood.label}` : '🙂 Good';
  }
}

function renderMoodWeek() {
  const moodRow = document.getElementById('moodRow');
  const weekDates = getWeekDates();
  const todayKey = formatDateKey(new Date());

  moodRow.innerHTML = '';

  weekDates.forEach((date, index) => {
    const dateKey = formatDateKey(date);
    const item = document.createElement('div');
    item.className = 'mood-day';

    const moodEntry = getMoodEntry(dateKey);
    const isToday = dateKey === todayKey;
    const isPast = getStartOfDay(date) < getStartOfDay(new Date());
    const isFuture = getStartOfDay(date) > getStartOfDay(new Date());

    let buttonText = '—';
    let buttonClassName = 'mood-circle empty';
    let buttonDisabled = true;
    let buttonTitle = 'No mood logged';

    if (moodEntry) {
      buttonText = moodEntry.emoji;
      buttonClassName = `mood-circle ${moodEntry.tone}`;
      buttonDisabled = isPast || !isToday;
      buttonTitle = isToday
        ? `Mood logged: ${moodEntry.label}. Click to update or remove.`
        : `Logged on ${date.toLocaleDateString('en-US', { month: 'short', day: 'numeric' })}`;
    } else if (isToday) {
      buttonText = '—';
      buttonClassName = 'mood-circle empty';
      buttonDisabled = false;
      buttonTitle = 'Add mood';
    } else if (isFuture) {
      buttonText = '—';
      buttonClassName = 'mood-circle empty';
      buttonDisabled = false;
      buttonTitle = 'Wait until tomorrow to add mood';
    } else {
      buttonText = '—';
      buttonClassName = 'mood-circle empty';
      buttonDisabled = true;
      buttonTitle = 'No mood logged';
    }

    const button = document.createElement('button');
    button.type = 'button';
    button.className = buttonClassName;
    button.textContent = buttonText;
    button.disabled = buttonDisabled;
    button.title = buttonTitle;

    if (isToday) {
      button.addEventListener('click', () => {
        moodTargetDateKey = dateKey;
        openMoodModal();
      });
    } else if (isFuture) {
      button.addEventListener('click', () => {
        moodTargetDateKey = dateKey;
        openMoodModal('notice');
      });
    }

    const label = document.createElement('span');
    label.className = 'mood-label';
    label.textContent = ['Mon', 'Tue', 'Wed', 'Thu', 'Fri', 'Sat', 'Sun'][index];

    item.appendChild(button);
    item.appendChild(label);
    moodRow.appendChild(item);
  });
}

function renderAssessmentCard() {
  const resumeBtn = document.getElementById('resumeBtn');
  const assessProgress = document.getElementById('assessProgress');
  const todayState = getAssessmentState();

  if (todayState && todayState.completed) {
    assessProgress.textContent = 'Completed today';
    resumeBtn.textContent = 'View results';
    resumeBtn.disabled = false;
    return;
  }

  if (todayState && Array.isArray(todayState.answers)) {
    const answered = todayState.answers.filter(answer => answer !== null).length;
    assessProgress.textContent = `${answered} of ${todayState.totalQuestions || 5} question answered`;
    resumeBtn.textContent = 'Resume';
    return;
  }

  assessProgress.textContent = 'Start your check-in';
  resumeBtn.textContent = 'Start';
}

function renderUpcomingAppointment() {
  const appointments = getAppointmentsByDate();
  const focusId = safeRead(DASHBOARD_FOCUS_KEY, null);

  if (focusId) {
    const focusIndex = appointments.findIndex(appointment => String(appointment.id) === String(focusId));
    if (focusIndex >= 0) {
      appointmentIndex = focusIndex;
    }
    localStorage.removeItem(DASHBOARD_FOCUS_KEY);
  }

  const apptName = document.querySelector('.appt-name');
  const apptMeta = document.querySelector('.appt-meta');
  const apptAvatar = document.querySelector('.appt-avatar');
  const rescheduleBtn = document.getElementById('rescheduleBtn');
  const cancelBtn = document.getElementById('cancelBtn');
  const joinBtn = document.getElementById('joinBtn');
  const prevBtn = document.getElementById('prevAppointmentBtn');
  const nextBtn = document.getElementById('nextAppointmentBtn');

  if (appointments.length === 0) {
    apptName.textContent = 'No appointment booked yet';
    apptMeta.textContent = 'Book a session to see your upcoming appointment here.';
    apptAvatar.textContent = 'NC';
    joinBtn.style.display = 'none';
    cancelBtn.style.display = 'none';
    rescheduleBtn.textContent = 'Book appointment';
    rescheduleBtn.style.display = 'inline-flex';

    if (prevBtn) prevBtn.style.display = 'none';
    if (nextBtn) nextBtn.style.display = 'none';
    return;
  }

  if (appointmentIndex >= appointments.length) {
    appointmentIndex = 0;
  }

  const appointment = appointments[appointmentIndex];

  apptName.textContent = appointment.counselorName;
  apptMeta.textContent = `${appointment.sessionType === 'virtual' ? 'Virtual' : appointment.sessionType} · ${formatDisplayDate(toDateTime(appointment.dateISO, appointment.time))}, ${appointment.time}`;
  apptAvatar.textContent = appointment.counselorInitials || 'PL';

  const startTime = toDateTime(appointment.dateISO, appointment.time);
  const now = new Date();
  const isToday = appointment.dateISO === formatDateKey(now);
  const isUpcoming = startTime >= now;
  const joinWindowStart = new Date(startTime.getTime() - 10 * 60 * 1000);
  const joinWindowEnd = new Date(startTime.getTime() - 5 * 60 * 1000);
  const isJoinTime = isToday && now >= joinWindowStart && now <= joinWindowEnd;

  rescheduleBtn.style.display = isToday ? 'none' : 'inline-flex';
  rescheduleBtn.textContent = 'Reschedule';
  cancelBtn.style.display = 'inline-flex';
  cancelBtn.disabled = !isUpcoming;
  cancelBtn.textContent = isUpcoming ? 'Cancel' : 'Cancelled';
  joinBtn.style.display = isJoinTime ? 'inline-flex' : 'none';

  if (prevBtn) {
    prevBtn.style.display = appointments.length > 1 ? 'inline-flex' : 'none';
    prevBtn.onclick = () => {
      appointmentIndex = (appointmentIndex - 1 + appointments.length) % appointments.length;
      renderUpcomingAppointment();
    };
  }

  if (nextBtn) {
    nextBtn.style.display = appointments.length > 1 ? 'inline-flex' : 'none';
    nextBtn.onclick = () => {
      appointmentIndex = (appointmentIndex + 1) % appointments.length;
      renderUpcomingAppointment();
    };
  }
}

function renderAppointmentStats() {
  const statCards = document.querySelectorAll('.stat-card');
  if (statCards.length < 4) {
    return;
  }

  const appointments = getAppointments();
  const sortedAppointments = [...appointments].sort((a, b) => {
    const timeA = toDateTime(a.dateISO, a.time).getTime();
    const timeB = toDateTime(b.dateISO, b.time).getTime();
    return timeA - timeB;
  });

  const latest = sortedAppointments[sortedAppointments.length - 1] || null;

  const firstAppointmentCard = statCards[1];
  const secondAppointmentCard = statCards[2];

  firstAppointmentCard.querySelector('.stat-label').textContent = 'Last Appointment';
  firstAppointmentCard.querySelector('.stat-value').textContent = 'Sep 12, 2026, 1: 00 PM';
  firstAppointmentCard.querySelector('.stat-sub').textContent = 'with M. Cruz, RPsy';
  firstAppointmentCard.querySelector('.stat-sub').classList.add('muted');

  secondAppointmentCard.querySelector('.stat-label').textContent = 'Appointments';
  secondAppointmentCard.querySelector('.stat-value').textContent = String(appointments.length || 0);
  secondAppointmentCard.querySelector('.stat-sub').textContent = latest
    ? `Last: ${formatDisplayDate(toDateTime(latest.dateISO, latest.time))}`
    : 'Last: none';
  secondAppointmentCard.querySelector('.stat-sub').classList.add('muted');
}

function renderRecommendations() {
  const mood = getTodayMood();
  const assessmentState = getAssessmentState();
  const primaryTag = assessmentState?.summary?.dominantArea || mood?.tone || 'stress';
  const matches = recommendedResources.filter(resource => {
    return resource.tags.includes(primaryTag) || resource.tags.includes(mood?.tone || 'good');
  });

  const items = matches.length > 0 ? matches : recommendedResources;
  const recommendationNodes = document.querySelectorAll('.reco-item');

  recommendationNodes.forEach((node, index) => {
    const resource = items[index % items.length];
    node.innerHTML = `
      <div class="reco-icon ${index % 2 === 0 ? 'blue' : 'green'}">${resource.icon}</div>
      <a class="reco-text" href="${resource.href}" target="_blank" rel="noopener noreferrer">${resource.title}</a>
    `;
  });
}

function openMoodModal(mode = 'picker') {
  const modal = document.getElementById('moodModal');
  const moodModalTitle = document.getElementById('moodModalTitle');
  const moodNotice = document.getElementById('moodNotice');
  const moodOptions = document.getElementById('moodOptions');
  const removeMoodBtn = document.getElementById('removeMoodBtn');

  const isNoticeMode = mode === 'notice';

  moodModalTitle.textContent = isNoticeMode ? 'Wait until tomorrow' : 'How are you feeling today?';
  moodOptions.classList.toggle('hidden', isNoticeMode);
  removeMoodBtn.classList.toggle('hidden', isNoticeMode);
  moodNotice.classList.toggle('hidden', !isNoticeMode);

  if (isNoticeMode) {
    moodNotice.textContent = 'Please wait until tomorrow to add a mood.';
  }

  modal.classList.remove('hidden');
  modal.setAttribute('aria-hidden', 'false');
}

function closeMoodModal() {
  const modal = document.getElementById('moodModal');
  modal.classList.add('hidden');
  modal.setAttribute('aria-hidden', 'true');
}

function removeMood() {
  const dateKey = moodTargetDateKey || formatDateKey(new Date());
  const moods = safeRead(STORAGE_KEYS.moods, []);
  const updated = moods.filter(mood => mood.dateKey !== dateKey);

  safeWrite(STORAGE_KEYS.moods, updated);
  pruneMoodData();
  renderMoodCards();
  renderMoodWeek();
  renderRecommendations();
  closeMoodModal();
}

function saveMood(option) {
  const dateKey = moodTargetDateKey || formatDateKey(new Date());
  const moods = safeRead(STORAGE_KEYS.moods, []);
  const updated = moods.filter(mood => mood.dateKey !== dateKey);
  updated.push({
    dateKey,
    label: option.label,
    emoji: option.emoji,
    tone: option.tone
  });

  safeWrite(STORAGE_KEYS.moods, updated);
  pruneMoodData();
  renderMoodCards();
  renderMoodWeek();
  renderRecommendations();
  closeMoodModal();
}

function bindMoodPicker() {
  const addMoodBtn = document.getElementById('addMoodBtn');
  const closeMoodModalBtn = document.getElementById('closeMoodModal');
  const removeMoodBtn = document.getElementById('removeMoodBtn');
  const moodModal = document.getElementById('moodModal');

  if (addMoodBtn) {
    addMoodBtn.addEventListener('click', () => {
      moodTargetDateKey = formatDateKey(new Date());
      openMoodModal();
    });
  }

  closeMoodModalBtn.addEventListener('click', closeMoodModal);

  if (removeMoodBtn) {
    removeMoodBtn.addEventListener('click', () => {
      removeMood();
    });
  }

  moodModal.addEventListener('click', event => {
    if (event.target === moodModal) {
      closeMoodModal();
    }
  });

  document.querySelectorAll('.mood-option').forEach(optionButton => {
    optionButton.addEventListener('click', () => {
      const option = {
        label: optionButton.dataset.label,
        emoji: optionButton.dataset.emoji,
        tone: optionButton.dataset.tone
      };
      saveMood(option);
    });
  });
}

function bindNavigation() {
  document.querySelectorAll('.nav-links a').forEach(link => {
    link.addEventListener('click', function (event) {
      event.preventDefault();
      document.querySelectorAll('.nav-links a').forEach(item => item.classList.remove('active'));
      this.classList.add('active');
    });
  });
}

function bindControls() {
  document.getElementById('bellBtn').addEventListener('click', function () {
    alert('You have new notifications.');
  });

  document.getElementById('avatarBtn').addEventListener('click', function () {
    alert('Profile menu goes here.');
  });

  const resumeBtn = document.getElementById('resumeBtn');
  resumeBtn.addEventListener('click', function () {
    window.location.href = '../Client_Assessment/index_4.html';
  });

  const rescheduleBtn = document.getElementById('rescheduleBtn');
  if (rescheduleBtn) {
    rescheduleBtn.addEventListener('click', function () {
      const appointments = getAppointmentsByDate();
      const appointment = appointments[appointmentIndex] || getUpcomingAppointment();
      if (appointment) {
        localStorage.setItem('mentalcare_reschedule_target', JSON.stringify({ id: appointment.id }));
        localStorage.setItem(DASHBOARD_FOCUS_KEY, String(appointment.id));
      }
      window.location.href = '../Client_Appointment/index_6.html';
    });
  }

  const cancelBtn = document.getElementById('cancelBtn');
  if (cancelBtn) {
    cancelBtn.addEventListener('click', function () {
      const appointments = getAppointmentsByDate();
      const appointment = appointments[appointmentIndex] || getUpcomingAppointment();

      if (!appointment) {
        return;
      }

      const remaining = getAppointments().filter(item => item.id !== appointment.id);
      safeWrite(STORAGE_KEYS.appointments, remaining);
      appointmentIndex = 0;
      renderUpcomingAppointment();
      renderAppointmentStats();
    });
  }
}

pruneMoodData();
renderGreeting();
renderMoodCards();
renderMoodWeek();
renderAssessmentCard();
renderUpcomingAppointment();
renderAppointmentStats();
renderRecommendations();
bindMoodPicker();
bindControls();
bindNavigation();

setInterval(() => {
  renderUpcomingAppointment();
  renderAppointmentStats();
}, 30000);
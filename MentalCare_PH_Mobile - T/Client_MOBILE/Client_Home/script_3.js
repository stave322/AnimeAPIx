const STORAGE_KEYS = {
  appointments: 'mentalcare_confirmed_appointments',
  moods: 'mentalcare_mood_log',
  assessment: 'mentalcare_daily_assessment'
};

const moodOptions = [
  { label: 'Great', emoji: '😆', tone: 'great' },
  { label: 'Good', emoji: '😄', tone: 'good' },
  { label: 'Okay', emoji: '😊', tone: 'ok' },
  { label: 'Low', emoji: '😕', tone: 'low' },
  { label: 'Sad', emoji: '😞', tone: 'low' }
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
    href: 'https://www.youtube.com/watch?v=86mT7VJp8M0'
  }
];

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

function getTodayMood() {
  const todayKey = formatDateKey(new Date());
  const moods = safeRead(STORAGE_KEYS.moods, []);
  return moods.find(entry => entry.dateKey === todayKey) || null;
}

function saveMood(optionLabel) {
  const todayKey = formatDateKey(new Date());
  const moods = safeRead(STORAGE_KEYS.moods, []);
  const updated = moods.filter(mood => mood.dateKey !== todayKey);
  const option = moodOptions.find(item => item.label === optionLabel) || moodOptions[1];

  updated.push({
    dateKey: todayKey,
    label: option.label,
    emoji: option.emoji,
    tone: option.tone
  });

  safeWrite(STORAGE_KEYS.moods, updated);
  renderMoodSelection();
  renderForYouCard();
}

function renderMoodSelection() {
  const todayMood = getTodayMood();
  document.querySelectorAll('.mood-emoji').forEach(button => {
    const active = todayMood && button.dataset.mood === todayMood.label;
    button.classList.toggle('selected', active);
  });
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
  const stored = safeRead(STORAGE_KEYS.appointments, []);
  return Array.isArray(stored)
    ? stored.filter(appointment => appointment && appointment.id !== 'sample-appointment').sort((a, b) => (b.createdAt || 0) - (a.createdAt || 0))
    : [];
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
  const userName = document.getElementById('userName');
  if (!userName) {
    return;
  }

  const hour = new Date().getHours();
  const greeting = hour >= 12 && hour < 18 ? 'Good Afternoon' : hour >= 18 ? 'Good Evening' : 'Good Morning';
  userName.textContent = `${greeting} 👋`;
}

function renderAssessmentProgress() {
  const progressFill = document.getElementById('progressFill');
  const progressLabel = document.getElementById('progressLabel');
  const state = getAssessmentState();

  if (!state) {
    if (progressFill) progressFill.style.width = '0%';
    if (progressLabel) progressLabel.textContent = '0 of 5 completed';
    return;
  }

  const answered = Array.isArray(state.answers) ? state.answers.filter(answer => answer !== null).length : 0;
  const total = state.totalQuestions || 5;

  if (progressFill) {
    progressFill.style.width = `${(answered / total) * 100}%`;
  }

  if (progressLabel) {
    progressLabel.textContent = state.completed
      ? `Completed today`
      : `${answered} of ${total} completed`;
  }
}

function renderAppointmentCard() {
  const appointment = getUpcomingAppointment();
  const apptName = document.querySelector('.appt-name');
  const apptMeta = document.querySelector('.appt-meta');
  const apptAvatar = document.querySelector('.appt-avatar');
  const joinBtn = document.getElementById('joinBtn');

  if (!appointment) {
    if (apptName) apptName.textContent = 'No appointment booked yet';
    if (apptMeta) apptMeta.textContent = 'Book a session to see your upcoming appointment here.';
    if (apptAvatar) apptAvatar.textContent = 'NC';
    if (joinBtn) {
      joinBtn.textContent = 'Book Session';
      joinBtn.disabled = false;
    }
    return;
  }

  if (apptName) apptName.textContent = appointment.counselorName;
  if (apptMeta) apptMeta.textContent = `${appointment.sessionType === 'virtual' ? 'Virtual' : appointment.sessionType} · ${appointment.dateISO} ${appointment.time}`;
  if (apptAvatar) apptAvatar.textContent = appointment.counselorInitials || 'PL';

  if (joinBtn) {
    const startTime = toDateTime(appointment.dateISO, appointment.time);
    const now = new Date();
    const isToday = appointment.dateISO === formatDateKey(now);
    const joinWindowStart = new Date(startTime.getTime() - 10 * 60 * 1000);
    const joinWindowEnd = new Date(startTime.getTime() - 5 * 60 * 1000);
    const isJoinTime = isToday && now >= joinWindowStart && now <= joinWindowEnd;

    joinBtn.textContent = isJoinTime ? 'Join Session' : 'Join Session';
    joinBtn.disabled = !isJoinTime;
  }
}

function renderForYouCard() {
  const forYouItem = document.getElementById('foryouItem');
  if (!forYouItem) {
    return;
  }

  const mood = getTodayMood();
  const assessmentState = getAssessmentState();
  const primaryTag = assessmentState?.summary?.dominantArea || mood?.tone || 'stress';

  const matches = recommendedResources.filter(resource => {
    return resource.tags.includes(primaryTag) || resource.tags.includes(mood?.tone || 'good');
  });

  const resource = matches[0] || recommendedResources[0];
  forYouItem.innerHTML = `
    <div class="foryou-icon">${resource.icon}</div>
    <div class="foryou-text">${resource.title}</div>
  `;
  forYouItem.dataset.href = resource.href;
}

function bindControls() {
  document.querySelectorAll('.mood-emoji').forEach(button => {
    button.addEventListener('click', () => {
      saveMood(button.dataset.mood);
    });
  });

  const joinBtn = document.getElementById('joinBtn');
  if (joinBtn) {
    joinBtn.addEventListener('click', () => {
      const appointment = getUpcomingAppointment();
      const startTime = appointment ? toDateTime(appointment.dateISO, appointment.time) : null;
      const now = new Date();
      const isToday = appointment && appointment.dateISO === formatDateKey(now);
      const joinWindowStart = appointment ? new Date(startTime.getTime() - 10 * 60 * 1000) : null;
      const joinWindowEnd = appointment ? new Date(startTime.getTime() - 5 * 60 * 1000) : null;

      if (appointment && isToday && now >= joinWindowStart && now <= joinWindowEnd) {
        window.location.href = '../Client_Virtual/index_9.html';
      } else if (appointment) {
        alert('Join session is available 10 to 5 minutes before the booked time.');
      } else {
        window.location.href = '../Client_BookAppoint/index_6.html';
      }
    });
  }

  const assessCard = document.getElementById('assessCard');
  if (assessCard) {
    assessCard.addEventListener('click', () => {
      window.location.href = '../Client_Assessment/index_4.html';
    });
  }

  const forYouItem = document.getElementById('foryouItem');
  if (forYouItem) {
    forYouItem.addEventListener('click', () => {
      const href = forYouItem.dataset.href || recommendedResources[0].href;
      window.open(href, '_blank', 'noopener,noreferrer');
    });
  }

  const bellBtn = document.getElementById('bellBtn');
  if (bellBtn) {
    bellBtn.addEventListener('click', function () {
      const bellDot = document.getElementById('bellDot');
      if (bellDot) bellDot.style.display = 'none';
      alert('You have new notifications.');
    });
  }

  const avatarBtn = document.getElementById('avatarBtn');
  if (avatarBtn) {
    avatarBtn.addEventListener('click', function () {
      alert('Profile menu goes here.');
    });
  }

  document.querySelectorAll('.tab-item').forEach(tab => {
    tab.addEventListener('click', function () {
      document.querySelectorAll('.tab-item').forEach(t => t.classList.remove('active'));
      this.classList.add('active');
    });
  });
}

function initHome() {
  renderGreeting();
  renderMoodSelection();
  renderAssessmentProgress();
  renderAppointmentCard();
  renderForYouCard();
  bindControls();
}

initHome();
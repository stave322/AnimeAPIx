const resources = [
  {
    type: 'Article',
    typeIcon: '📄',
    iconClass: 'article',
    title: '5 Grounding techniques for Stress',
    desc: 'Simple, science-backed ways to feel calmer in the moment.',
    tags: ['Stress', 'Anxiety'],
    link: 'https://www.healthline.com/health/grounding-techniques'
  },
  {
    type: 'Video - 5 Min',
    typeIcon: '🐻',
    iconClass: 'video',
    title: 'Guided breathing exercise',
    desc: 'A short, guided session to reset before your day.',
    tags: ['Stress', 'Anxiety'],
    link: 'https://www.youtube.com/watch?v=LiUnFJ8P4gM'
  },
  {
    type: 'Article',
    typeIcon: '📄',
    iconClass: 'article',
    title: 'Understanding academic burnout',
    desc: 'Recognize the signs early and what you can do.',
    tags: ['Stress', 'Self-Care'],
    link: 'https://www.nimh.nih.gov/health/topics/caring-for-your-mental-health'
  },
  {
    type: 'Audio - 10 Min',
    typeIcon: '🎵',
    iconClass: 'audio',
    title: 'Sleep wind-down meditation',
    desc: 'Ease your mind before bedtime.',
    tags: ['Sleep'],
    link: 'https://www.youtube.com/watch?v=_3fvhTO3pLM'
  },
  {
    type: 'Article',
    typeIcon: '📄',
    iconClass: 'article',
    title: 'Talking to family about mental health',
    desc: 'Starting the conversation with care.',
    tags: ['Relationship', 'Self-Care'],
    link: 'https://www.nimh.nih.gov/health/find-help'
  },
  {
    type: 'Video - 8 Min',
    typeIcon: '🐻',
    iconClass: 'video',
    title: 'Building a self-care routine',
    desc: 'Small habits that support long-term well-being.',
    tags: ['Self-Care'],
    link: 'outube.com/watch?v=DBKdro1AjdA'
  }
];

const grid = document.getElementById('resourceGrid');
const chips = document.querySelectorAll('.chip');
let activeFilter = 'All';

function safeRead(key, fallback) {
  try {
    const stored = localStorage.getItem(key);
    return stored ? JSON.parse(stored) : fallback;
  } catch (error) {
    return fallback;
  }
}

function formatDateKey(date) {
  const year = date.getFullYear();
  const month = String(date.getMonth() + 1).padStart(2, '0');
  const day = String(date.getDate()).padStart(2, '0');
  return `${year}-${month}-${day}`;
}

function getRecommendedResources() {
  const moodEntries = safeRead('mentalcare_mood_log', []);
  const todayMood = moodEntries.find(entry => entry.dateKey === formatDateKey(new Date())) || null;
  const assessmentState = safeRead('mentalcare_daily_assessment', null);
  const dominantArea = assessmentState?.summary?.dominantArea || todayMood?.tone || 'Stress';

  const scored = resources.map(resource => {
    const score = resource.tags.some(tag => tag.toLowerCase() === dominantArea.toLowerCase())
      ? 2
      : 0 + (todayMood && resource.tags.some(tag => tag.toLowerCase() === todayMood.label.toLowerCase()) ? 1 : 0);

    return { resource, score };
  });

  const sorted = scored.sort((a, b) => b.score - a.score).map(entry => entry.resource);
  return sorted.length ? sorted : resources;
}

function renderResources() {
  grid.innerHTML = '';
  const visibleResources = activeFilter === 'All'
    ? getRecommendedResources()
    : resources.filter(r => r.tags.includes(activeFilter));

  if (visibleResources.length === 0) {
    const empty = document.createElement('div');
    empty.className = 'empty-state';
    empty.textContent = `No resources found for "${activeFilter}" yet.`;
    grid.appendChild(empty);
    return;
  }

  visibleResources.forEach(r => {
    const card = document.createElement('div');
    card.className = 'resource-card';
    card.innerHTML = `
      <div class="resource-icon ${r.iconClass}">${r.typeIcon}</div>
      <div class="resource-type">${r.type}</div>
      <h3 class="resource-title">${r.title}</h3>
      <p class="resource-desc">${r.desc}</p>
    `;
    card.addEventListener('click', () => {
      window.open(r.link, '_blank', 'noopener,noreferrer');
    });
    grid.appendChild(card);
  });
}

chips.forEach(chip => {
  chip.addEventListener('click', function () {
    chips.forEach(c => c.classList.remove('active'));
    this.classList.add('active');
    activeFilter = this.dataset.filter;
    renderResources();
  });
});

document.querySelectorAll('.nav-links a').forEach(link => {
  link.addEventListener('click', function (e) {
    e.preventDefault();
    document.querySelectorAll('.nav-links a').forEach(l => l.classList.remove('active'));
    this.classList.add('active');
  });
});

document.getElementById('bellBtn').addEventListener('click', () => {
  alert('You have new notifications.');
});

document.getElementById('avatarBtn').addEventListener('click', () => {
  alert('Profile menu goes here.');
});

renderResources();
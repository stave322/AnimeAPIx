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
    link: 'https://www.youtube.com/watch?v=DBKdro1AjdA'
  }
];

const resourceList = document.getElementById('resourceList');
const chips = document.querySelectorAll('.chip');
const searchBtn = document.getElementById('searchBtn');
const searchBar = document.getElementById('searchBar');
const searchInput = document.getElementById('searchInput');

let activeFilter = 'All';
let searchTerm = '';

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

function matchesFilter(resource, filter) {
  if (filter === 'All') {
    return true;
  }

  return resource.tags.some(tag => tag.toLowerCase() === filter.toLowerCase());
}

function getRecommendedResources() {
  const moodEntries = safeRead('mentalcare_mood_log', []);
  const todayMood = moodEntries.find(entry => entry.dateKey === formatDateKey(new Date())) || null;
  const assessmentState = safeRead('mentalcare_daily_assessment', null);
  const dominantArea = assessmentState?.summary?.dominantArea || todayMood?.tone || 'Stress';

  const scored = resources.map(resource => {
    const resourceTags = resource.tags.map(tag => tag.toLowerCase());
    const dominantScore = resourceTags.includes(dominantArea.toLowerCase()) ? 2 : 0;
    const moodScore = todayMood && resourceTags.includes((todayMood.label || '').toLowerCase()) ? 1 : 0;

    return { resource, score: dominantScore + moodScore };
  });

  const sorted = scored.sort((a, b) => b.score - a.score).map(entry => entry.resource);
  return sorted.length ? sorted : resources;
}

function renderResources() {
  resourceList.innerHTML = '';

  let filtered = activeFilter === 'All'
    ? getRecommendedResources()
    : resources.filter(resource => matchesFilter(resource, activeFilter));

  if (searchTerm.trim().length > 0) {
    filtered = filtered.filter(resource => resource.title.toLowerCase().includes(searchTerm.toLowerCase()));
  }

  if (filtered.length === 0) {
    const empty = document.createElement('div');
    empty.className = 'empty-state';
    empty.textContent = 'No resources found.';
    resourceList.appendChild(empty);
    return;
  }

  filtered.forEach(resource => {
    const card = document.createElement('div');
    card.className = 'resource-card';
    card.innerHTML = `
      <div class="resource-icon ${resource.iconClass}">${resource.typeIcon}</div>
      <div class="resource-content">
        <div class="resource-type">${resource.type}</div>
        <div class="resource-title">${resource.title}</div>
        <div class="resource-desc">${resource.desc}</div>
      </div>
    `;

    card.addEventListener('click', () => {
      window.open(resource.link, '_blank', 'noopener,noreferrer');
    });

    resourceList.appendChild(card);
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

searchBtn.addEventListener('click', () => {
  searchBar.classList.toggle('show');
  if (searchBar.classList.contains('show')) {
    searchInput.focus();
  }
});

searchInput.addEventListener('input', function () {
  searchTerm = this.value;
  renderResources();
});

document.querySelectorAll('.tab-item').forEach(tab => {
  tab.addEventListener('click', function () {
    document.querySelectorAll('.tab-item').forEach(t => t.classList.remove('active'));
    this.classList.add('active');
  });
});

renderResources();
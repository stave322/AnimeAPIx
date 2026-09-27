const content = [
  { title: '5 grounding techniques for stress', type: 'Article', category: 'Stress', status: 'published', updated: 'Aug 10, 2026' },
  { title: 'Guided breathing exercise', type: 'Video', category: 'Anxiety', status: 'published', updated: 'Aug 8, 2026' },
  { title: 'Managing exam stress', type: 'Article', category: 'Stress', status: 'draft', updated: 'Aug 14, 2026' },
  { title: 'Sleep wind-down meditation', type: 'Audio', category: 'Sleep', status: 'published', updated: 'Aug 2, 2026' }
];

const contentBody = document.getElementById('contentBody');
const chips = document.querySelectorAll('.chip[data-cat]');
let activeCategory = 'All Categories';

function renderContent() {
  const filtered = activeCategory === 'All Categories'
    ? content
    : content.filter(c => c.category === activeCategory);

  contentBody.innerHTML = '';

  if (filtered.length === 0) {
    const row = document.createElement('tr');
    row.className = 'empty-row';
    row.innerHTML = `<td colspan="6">No content in this category yet.</td>`;
    contentBody.appendChild(row);
    return;
  }

  filtered.forEach(c => {
    const row = document.createElement('tr');
    const isDraft = c.status === 'draft';
    row.innerHTML = `
      <td>${c.title}</td>
      <td>${c.type}</td>
      <td>${c.category}</td>
      <td><span class="status-pill ${c.status}">${isDraft ? 'Draft' : 'Published'}</span></td>
      <td>${c.updated}</td>
      <td><button class="action-btn ${isDraft ? 'review' : ''}">${isDraft ? 'Review' : 'Edit'}</button></td>
    `;
    row.querySelector('.action-btn').addEventListener('click', () => {
      alert(isDraft ? `Reviewing draft: ${c.title}` : `Editing: ${c.title}`);
    });
    contentBody.appendChild(row);
  });
}

chips.forEach(chip => {
  chip.addEventListener('click', function () {
    chips.forEach(c => c.classList.remove('active'));
    this.classList.add('active');
    activeCategory = this.dataset.cat;
    renderContent();
  });
});

document.getElementById('addCategoryBtn').addEventListener('click', () => {
  const name = prompt('New category name:');
  if (name && name.trim()) {
    alert(`Category "${name.trim()}" created.`);
  }
});

document.getElementById('newResourceBtn').addEventListener('click', () => {
  alert('Opening "New Resource" editor...');
});

document.querySelectorAll('.nav-item').forEach(item => {
  item.addEventListener('click', function () {
    document.querySelectorAll('.nav-item').forEach(i => i.classList.remove('active'));
    this.classList.add('active');
  });
});

renderContent();
const weeklyData = [
  { label: 'Wk1', value: 58 },
  { label: 'Wk2', value: 64 },
  { label: 'Wk3', value: 55 },
  { label: 'Wk4', value: 92 },
  { label: 'Wk5', value: 60 },
  { label: 'Wk6', value: 78 }
];

const barChart = document.getElementById('barChart');
let selectedIndex = 3; // Wk4 highlighted, matching screenshot

function renderBarChart() {
  barChart.innerHTML = '';
  const maxValue = Math.max(...weeklyData.map(d => d.value));

  weeklyData.forEach((d, i) => {
    const col = document.createElement('div');
    col.className = 'bar-col' + (i === selectedIndex ? ' selected' : '');

    const heightPct = (d.value / maxValue) * 100;

    col.innerHTML = `
      <span class="bar-value">${d.value}</span>
      <div class="bar ${i === selectedIndex ? 'active' : ''}" style="height:${heightPct}%"></div>
      <span class="bar-label">${d.label}</span>
    `;

    col.addEventListener('click', () => {
      selectedIndex = i;
      renderBarChart();
    });

    barChart.appendChild(col);
  });
}

const concerns = [
  { name: 'Academic stress', share: 68 },
  { name: 'Anxiety', share: 52 },
  { name: 'Sleep issues', share: 41 }
];

const concernList = document.getElementById('concernList');

function renderConcerns() {
  concernList.innerHTML = '';
  concerns.forEach(c => {
    const row = document.createElement('div');
    row.className = 'concern-row';
    row.innerHTML = `
      <span class="concern-name">${c.name}</span>
      <div class="concern-bar-track">
        <div class="concern-bar-fill" style="width:${c.share}%"></div>
      </div>
    `;
    concernList.appendChild(row);
  });
}

document.getElementById('exportBtn').addEventListener('click', () => {
  alert('Exporting report as CSV...');
});

document.querySelectorAll('.nav-item').forEach(item => {
  item.addEventListener('click', function () {
    document.querySelectorAll('.nav-item').forEach(i => i.classList.remove('active'));
    this.classList.add('active');
  });
});

renderBarChart();
renderConcerns();
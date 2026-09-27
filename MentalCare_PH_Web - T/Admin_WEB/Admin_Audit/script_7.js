const logs = [
  { logId: '#1042', admin: 'System Admin', action: 'create', table: 'Counselors', recordId: 'COU-0031', details: 'Approved counselor account for J. Torres, RGC', timestamp: 'Aug 14, 2026 · 9:12 AM' },
  { logId: '#1041', admin: 'System Admin', action: 'update', table: 'Educational_Content', recordId: 'CNT-0067', details: 'Published "Managing exam stress"', timestamp: 'Aug 14, 2026 · 8:40 AM' },
  { logId: '#1040', admin: 'System Admin', action: 'update', table: 'Users', recordId: 'USR-0512', details: 'Changed role permissions for Guidance Counselor', timestamp: 'Aug 13, 2026 · 4:05 PM' },
  { logId: '#1039', admin: 'System Admin', action: 'suspend', table: 'Users', recordId: 'USR-0288', details: 'Suspended account for Marco Santos (policy violation)', timestamp: 'Aug 13, 2026 · 2:22 PM' },
  { logId: '#1038', admin: 'System Admin', action: 'delete', table: 'Educational_Content', recordId: 'CNT-0059', details: 'Removed outdated article "Old coping guide"', timestamp: 'Aug 12, 2026 · 11:50 AM' },
  { logId: '#1037', admin: 'System Admin', action: 'create', table: 'Users', recordId: 'USR-0511', details: 'Created new client account for Rico Bautista', timestamp: 'Aug 12, 2026 · 9:03 AM' },
  { logId: '#1036', admin: 'System Admin', action: 'update', table: 'System_Settings', recordId: '—', details: 'Enabled required 2FA for admin accounts', timestamp: 'Aug 11, 2026 · 3:15 PM' }
];

const actionLabels = { create: 'Create', update: 'Update', suspend: 'Suspend', delete: 'Delete' };

const logsBody = document.getElementById('logsBody');
const searchInput = document.getElementById('searchInput');
const actionFilter = document.getElementById('actionFilter');
const tableFilter = document.getElementById('tableFilter');
const dateFilter = document.getElementById('dateFilter');

function renderLogs() {
  const searchTerm = searchInput.value.trim().toLowerCase();
  const actionVal = actionFilter.value;
  const tableVal = tableFilter.value;
  const dateVal = dateFilter.value;

  const filtered = logs.filter(l => {
    const matchesSearch =
      l.admin.toLowerCase().includes(searchTerm) ||
      l.details.toLowerCase().includes(searchTerm) ||
      l.recordId.toLowerCase().includes(searchTerm);
    const matchesAction = actionVal === 'all' || l.action === actionVal;
    const matchesTable = tableVal === 'all' || l.table === tableVal;
    const matchesDate = !dateVal || l.timestamp.startsWith(dateVal);
    return matchesSearch && matchesAction && matchesTable && matchesDate;
  });

  logsBody.innerHTML = '';

  if (filtered.length === 0) {
    const row = document.createElement('tr');
    row.className = 'empty-row';
    row.innerHTML = `<td colspan="7">No log entries match your filters.</td>`;
    logsBody.appendChild(row);
    return;
  }

  filtered.forEach(l => {
    const row = document.createElement('tr');
    row.innerHTML = `
      <td>${l.logId}</td>
      <td>${l.admin}</td>
      <td><span class="action-pill ${l.action}">${actionLabels[l.action]}</span></td>
      <td>${l.table}</td>
      <td>${l.recordId}</td>
      <td class="details-cell">${l.details}</td>
      <td class="time-cell">${l.timestamp}</td>
    `;
    logsBody.appendChild(row);
  });
}

searchInput.addEventListener('input', renderLogs);
actionFilter.addEventListener('change', renderLogs);
tableFilter.addEventListener('change', renderLogs);
dateFilter.addEventListener('change', renderLogs);

document.getElementById('exportBtn').addEventListener('click', () => {
  alert('Exporting audit logs as CSV...');
});

document.querySelectorAll('.nav-item').forEach(item => {
  item.addEventListener('click', function () {
    document.querySelectorAll('.nav-item').forEach(i => i.classList.remove('active'));
    this.classList.add('active');
  });
});

renderLogs();
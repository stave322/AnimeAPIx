const permissions = [
  { name: 'View educational resources', client: true, counselor: true, admin: true },
  { name: 'Book appointments', client: true, counselor: false, admin: false },
  { name: 'View own assessment results', client: true, counselor: false, admin: false },
  { name: 'Review client assessment results', client: false, counselor: true, admin: true },
  { name: 'Manage appointment schedule', client: false, counselor: true, admin: false },
  { name: 'Manage user accounts', client: false, counselor: false, admin: true },
  { name: 'Publish educational content', client: false, counselor: false, admin: true },
  { name: 'View system audit logs', client: false, counselor: false, admin: true }
];

const permBody = document.getElementById('permBody');

function renderPermissions() {
  permBody.innerHTML = '';
  permissions.forEach((p, i) => {
    const row = document.createElement('tr');
    row.innerHTML = `
      <td>${p.name}</td>
      <td class="perm-cell"><button class="perm-toggle ${p.client ? 'granted' : 'denied'}" data-row="${i}" data-role="client">${p.client ? '✓' : '–'}</button></td>
      <td class="perm-cell"><button class="perm-toggle ${p.counselor ? 'granted' : 'denied'}" data-row="${i}" data-role="counselor">${p.counselor ? '✓' : '–'}</button></td>
      <td class="perm-cell"><button class="perm-toggle ${p.admin ? 'granted' : 'denied'}" data-row="${i}" data-role="admin">${p.admin ? '✓' : '–'}</button></td>
    `;
    permBody.appendChild(row);
  });

  document.querySelectorAll('.perm-toggle').forEach(btn => {
    btn.addEventListener('click', function () {
      const rowIndex = parseInt(this.dataset.row, 10);
      const role = this.dataset.role;
      permissions[rowIndex][role] = !permissions[rowIndex][role];
      renderPermissions();
    });
  });
}

document.getElementById('newRoleBtn').addEventListener('click', () => {
  const roleName = prompt('New role name:');
  if (roleName && roleName.trim()) {
    alert(`Role "${roleName.trim()}" created. Configure its permissions next.`);
  }
});

document.querySelectorAll('.nav-item').forEach(item => {
  item.addEventListener('click', function () {
    document.querySelectorAll('.nav-item').forEach(i => i.classList.remove('active'));
    this.classList.add('active');
  });
});

renderPermissions();
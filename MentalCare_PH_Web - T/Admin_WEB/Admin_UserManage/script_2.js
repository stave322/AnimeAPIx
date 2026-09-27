const users = [
  { name: 'Juana Dela Cruz', email: 'juana.delacruz@atc.edu.ph', role: 'client', status: 'active', joined: 'Feb 2026' },
  { name: 'Ana Reyes, RPsy', email: 'areyes@mentalcareph.com', role: 'counselor', status: 'active', joined: 'Jan 2026' },
  { name: 'Marco Santos', email: 'msantos@atc.edu.ph', role: 'client', status: 'suspended', joined: 'Mar 2026' },
  { name: 'Maria Cruz, RPsy', email: 'mcruz@mentalcareph.com', role: 'counselor', status: 'pending', joined: 'Aug 2026' },
  { name: 'Rico Bautista', email: 'rbautista@atc.edu.ph', role: 'client', status: 'active', joined: 'May 2026' }
];

const usersBody = document.getElementById('usersBody');
const searchInput = document.getElementById('searchInput');
const roleFilter = document.getElementById('roleFilter');
const statusFilter = document.getElementById('statusFilter');

const roleLabels = { client: 'Client', counselor: 'Counselor' };
const statusLabels = { active: 'Active', suspended: 'Suspended', pending: 'Pending' };

function renderUsers() {
  const searchTerm = searchInput.value.trim().toLowerCase();
  const roleVal = roleFilter.value;
  const statusVal = statusFilter.value;

  const filtered = users.filter(u => {
    const matchesSearch = u.name.toLowerCase().includes(searchTerm) || u.email.toLowerCase().includes(searchTerm);
    const matchesRole = roleVal === 'all' || u.role === roleVal;
    const matchesStatus = statusVal === 'all' || u.status === statusVal;
    return matchesSearch && matchesRole && matchesStatus;
  });

  usersBody.innerHTML = '';

  if (filtered.length === 0) {
    const row = document.createElement('tr');
    row.className = 'empty-row';
    row.innerHTML = `<td colspan="6">No users match your filters.</td>`;
    usersBody.appendChild(row);
    return;
  }

  filtered.forEach(u => {
    const row = document.createElement('tr');
    row.innerHTML = `
      <td>${u.name}</td>
      <td>${u.email}</td>
      <td>${roleLabels[u.role]}</td>
      <td><span class="status-pill ${u.status}">${statusLabels[u.status]}</span></td>
      <td>${u.joined}</td>
      <td><button class="manage-btn">Manage</button></td>
    `;
    row.querySelector('.manage-btn').addEventListener('click', () => {
      alert(`Managing account: ${u.name}`);
    });
    usersBody.appendChild(row);
  });
}

searchInput.addEventListener('input', renderUsers);
roleFilter.addEventListener('change', renderUsers);
statusFilter.addEventListener('change', renderUsers);

document.getElementById('addUserBtn').addEventListener('click', () => {
  alert('Opening "Add User" form...');
});

document.querySelectorAll('.nav-item').forEach(item => {
  item.addEventListener('click', function () {
    document.querySelectorAll('.nav-item').forEach(i => i.classList.remove('active'));
    this.classList.add('active');
  });
});

renderUsers();
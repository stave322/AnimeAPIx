const form = document.getElementById('profileForm');
const fullName = document.getElementById('fullName');
const email = document.getElementById('email');
const phone = document.getElementById('phone');
const saveMsg = document.getElementById('saveMsg');
const headerName = document.getElementById('headerName');
const headerAvatar = document.getElementById('headerAvatar');

const fields = {
  fullNameField: document.getElementById('fullNameField'),
  emailField: document.getElementById('emailField'),
  phoneField: document.getElementById('phoneField')
};

function isValidEmail(value) {
  return /^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(value);
}

function isValidPhone(value) {
  return /^[0-9+\-\s]{7,17}$/.test(value);
}

function getInitials(name) {
  const parts = name.trim().split(/\s+/);
  if (parts.length === 1) return parts[0].slice(0, 2).toUpperCase();
  return (parts[0][0] + parts[parts.length - 1][0]).toUpperCase();
}

function clearErrors() {
  Object.values(fields).forEach(f => f.classList.remove('field-error'));
  saveMsg.textContent = '';
}

form.addEventListener('submit', function (e) {
  e.preventDefault();
  clearErrors();

  let valid = true;

  if (fullName.value.trim().length === 0) {
    fields.fullNameField.classList.add('field-error');
    valid = false;
  }

  if (!isValidEmail(email.value.trim())) {
    fields.emailField.classList.add('field-error');
    valid = false;
  }

  if (!isValidPhone(phone.value.trim())) {
    fields.phoneField.classList.add('field-error');
    valid = false;
  }

  if (!valid) {
    saveMsg.style.color = '#d64545';
    saveMsg.textContent = 'Please fix the errors above.';
    return;
  }

  headerName.textContent = fullName.value.trim();
  headerAvatar.textContent = getInitials(fullName.value.trim());

  saveMsg.style.color = 'var(--teal)';
  saveMsg.textContent = 'Changes saved successfully.';
});

document.getElementById('privacyBtn').addEventListener('click', () => {
  alert('Opening privacy & data settings...');
});

document.querySelectorAll('.tab-item').forEach(tab => {
  tab.addEventListener('click', function () {
    document.querySelectorAll('.tab-item').forEach(t => t.classList.remove('active'));
    this.classList.add('active');
  });
});
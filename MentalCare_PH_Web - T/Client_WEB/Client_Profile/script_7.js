const form = document.getElementById('profileForm');
const fullName = document.getElementById('fullName');
const email = document.getElementById('email');
const phone = document.getElementById('phone');
const dob = document.getElementById('dob');
const bio = document.getElementById('bio');
const saveMsg = document.getElementById('saveMsg');
const headerName = document.getElementById('headerName');
const headerAvatar = document.getElementById('headerAvatar');
const avatarBtn = document.getElementById('avatarBtn');

const fields = {
  fullNameField: document.getElementById('fullNameField'),
  emailField: document.getElementById('emailField'),
  phoneField: document.getElementById('phoneField'),
  dobField: document.getElementById('dobField')
};

// Store original values for Cancel
const originalValues = {
  fullName: fullName.value,
  email: email.value,
  phone: phone.value,
  dob: dob.value,
  bio: bio.value
};

function isValidEmail(value) {
  return /^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(value);
}

function isValidPhone(value) {
  return /^[0-9+\-\s]{7,15}$/.test(value);
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

  if (dob.value.trim().length === 0) {
    fields.dobField.classList.add('field-error');
    valid = false;
  }

  if (!valid) {
    saveMsg.style.color = '#d64545';
    saveMsg.textContent = 'Please fix the errors above.';
    return;
  }

  // Update header + avatar with new name
  headerName.textContent = fullName.value.trim();
  const initials = getInitials(fullName.value.trim());
  headerAvatar.textContent = initials;
  avatarBtn.textContent = initials;

  saveMsg.style.color = 'var(--teal)';
  saveMsg.textContent = 'Changes saved successfully.';
});

document.getElementById('cancelBtn').addEventListener('click', function () {
  fullName.value = originalValues.fullName;
  email.value = originalValues.email;
  phone.value = originalValues.phone;
  dob.value = originalValues.dob;
  bio.value = originalValues.bio;
  clearErrors();
  saveMsg.style.color = 'var(--text-muted)';
  saveMsg.textContent = 'Changes discarded.';
});

document.getElementById('privacyBtn').addEventListener('click', function () {
  alert('Opening privacy settings...');
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

avatarBtn.addEventListener('click', () => {
  alert('Profile menu goes here.');
});
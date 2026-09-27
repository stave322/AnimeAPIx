const form = document.getElementById('signupForm');
const fullName = document.getElementById('fullName');
const email = document.getElementById('email');
const password = document.getElementById('password');
const role = document.getElementById('role');
const consent = document.getElementById('consent');

const fields = {
  fullNameField: document.getElementById('fullNameField'),
  emailField: document.getElementById('emailField'),
  passwordField: document.getElementById('passwordField'),
  roleField: document.getElementById('roleField'),
  consentField: document.getElementById('consentField')
};

const formMsg = document.getElementById('formMsg');
const createBtn = document.getElementById('createBtn');

function isValidEmail(value) {
  return /^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(value);
}

function clearErrors() {
  Object.values(fields).forEach(f => f.classList.remove('field-error'));
  formMsg.textContent = '';
  formMsg.className = 'form-msg';
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

  if (password.value.length < 8) {
    fields.passwordField.classList.add('field-error');
    valid = false;
  }

  if (!role.value) {
    fields.roleField.classList.add('field-error');
    valid = false;
  }

  if (!consent.checked) {
    fields.consentField.classList.add('field-error');
    valid = false;
  }

  if (!valid) {
    formMsg.textContent = 'Please fix the errors above.';
    formMsg.classList.add('error');
    return;
  }

  createBtn.disabled = true;
  createBtn.textContent = 'Creating account...';
  formMsg.textContent = '';

  setTimeout(function () {
    createBtn.disabled = false;
    createBtn.textContent = 'Create Account';
    formMsg.textContent = 'Account created! Redirecting...';
    formMsg.classList.add('success');
    window.location.href = '../Client_Home/index_3.html';
  }, 1200);
});

document.getElementById('privacyLink').addEventListener('click', function (e) {
  e.preventDefault();
  formMsg.textContent = 'Privacy Policy prototype coming soon.';
  formMsg.className = 'form-msg success';
});

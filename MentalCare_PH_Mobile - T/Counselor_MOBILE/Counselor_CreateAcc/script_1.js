const form = document.getElementById('requestForm');
const fullName = document.getElementById('fullName');
const email = document.getElementById('email');
const license = document.getElementById('license');
const password = document.getElementById('password');

const fields = {
  fullNameField: document.getElementById('fullNameField'),
  emailField: document.getElementById('emailField'),
  licenseField: document.getElementById('licenseField'),
  passwordField: document.getElementById('passwordField')
};

const formMsg = document.getElementById('formMsg');
const submitBtn = document.getElementById('submitBtn');

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

  if (license.value.trim().length === 0) {
    fields.licenseField.classList.add('field-error');
    valid = false;
  }

  if (password.value.length < 8) {
    fields.passwordField.classList.add('field-error');
    valid = false;
  }

  if (!valid) {
    formMsg.textContent = 'Please fix the errors above.';
    formMsg.classList.add('error');
    return;
  }

  submitBtn.disabled = true;
  submitBtn.textContent = 'Submitting...';
  formMsg.textContent = '';

  setTimeout(function () {
    submitBtn.disabled = false;
    submitBtn.textContent = 'Submit for verification';
    formMsg.textContent = 'Request submitted! An administrator will verify your license.';
    formMsg.classList.add('success');
    window.location.href = '../Counselor_Login/index.html';
  }, 1200);
});
const form = document.getElementById('loginForm');
const emailInput = document.getElementById('email');
const passwordInput = document.getElementById('password');
const twofaInput = document.getElementById('twofa');
const emailField = document.getElementById('emailField');
const passwordField = document.getElementById('passwordField');
const twofaField = document.getElementById('twofaField');
const formMsg = document.getElementById('formMsg');
const loginBtn = document.getElementById('loginBtn');

function isValidEmail(value) {
  return /^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(value);
}

function clearErrors() {
  emailField.classList.remove('field-error');
  passwordField.classList.remove('field-error');
  twofaField.classList.remove('field-error');
  formMsg.textContent = '';
  formMsg.className = 'form-msg';
}

twofaInput.addEventListener('input', function () {
  this.value = this.value.replace(/\D/g, '').slice(0, 6);
});

form.addEventListener('submit', function (e) {
  e.preventDefault();
  clearErrors();

  let valid = true;

  if (!isValidEmail(emailInput.value.trim())) {
    emailField.classList.add('field-error');
    valid = false;
  }

  if (passwordInput.value.trim().length === 0) {
    passwordField.classList.add('field-error');
    valid = false;
  }

  if (twofaInput.value.trim().length !== 6) {
    twofaField.classList.add('field-error');
    valid = false;
  }

  if (!valid) {
    formMsg.textContent = 'Please fix the errors above.';
    formMsg.classList.add('error');
    return;
  }

  loginBtn.disabled = true;
  loginBtn.textContent = 'Verifying...';
  formMsg.textContent = '';

  setTimeout(function () {
    loginBtn.disabled = false;
    loginBtn.textContent = 'Log In';
    formMsg.textContent = 'Access granted. Redirecting to admin console...';
    formMsg.classList.add('success');
    setTimeout(function () {
      window.location.href = '../Admin_Dashboard/index_1.html';
    }, 500);
  }, 1200);
});
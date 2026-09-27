const form = document.getElementById('loginForm');
const emailInput = document.getElementById('email');
const passwordInput = document.getElementById('password');
const emailField = document.getElementById('emailField');
const passwordField = document.getElementById('passwordField');
const formMsg = document.getElementById('formMsg');
const loginBtn = document.getElementById('loginBtn');

function isValidEmail(value) {
  return /^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(value);
}

function clearErrors() {
  emailField.classList.remove('field-error');
  passwordField.classList.remove('field-error');
  formMsg.textContent = '';
  formMsg.className = 'form-msg';
}

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

  if (!valid) {
    formMsg.textContent = 'Please fix the errors above.';
    formMsg.classList.add('error');
    return;
  }

  loginBtn.disabled = true;
  loginBtn.textContent = 'Logging in...';
  formMsg.textContent = '';

  setTimeout(function () {
    loginBtn.disabled = false;
    loginBtn.textContent = 'Log In';
    formMsg.textContent = 'Login successful! Redirecting...';
    formMsg.classList.add('success');
    window.location.href = '../Client_Home/index_3.html';
  }, 1200);
});

document.getElementById('forgotLink').addEventListener('click', function (e) {
  e.preventDefault();
  formMsg.textContent = 'Password reset prototype coming soon.';
  formMsg.className = 'form-msg success';
});
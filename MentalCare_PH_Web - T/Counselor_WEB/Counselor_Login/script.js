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
    formMsg.textContent = 'Login successful! Redirecting to dashboard...';
    formMsg.classList.add('success');
    setTimeout(function () {
      window.location.href = '../Counselor_Dashboard/index_2.html';
    }, 500);
  }, 1200);
});

document.getElementById('forgotLink').addEventListener('click', function (e) {
  e.preventDefault();
  alert('Password reset flow goes here.');
});

document.getElementById('createLink').addEventListener('click', function (e) {
  e.preventDefault();
  window.location.href = '../Counselor_CreateAcc/index_1.html';
});

document.getElementById('userLink').addEventListener('click', function (e) {
  e.preventDefault();
  window.location.href = '../../Client_WEB/Client_Login/index.html';
});
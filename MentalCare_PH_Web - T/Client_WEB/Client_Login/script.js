
function getUsers() {
  try {
    return JSON.parse(localStorage.getItem('mc_users')) || [];
  } catch (e) {
    return [];
  }
}

function saveUsers(users) {
  localStorage.setItem('mc_users', JSON.stringify(users));
}

function getActiveEmail() {
  return localStorage.getItem('mc_session') || sessionStorage.getItem('mc_session');
}

function setSession(email, remember) {
  if (remember) {
    localStorage.setItem('mc_session', email);
    sessionStorage.removeItem('mc_session');
  } else {
    sessionStorage.setItem('mc_session', email);
    localStorage.removeItem('mc_session');
  }
}

/* If a session already exists, skip straight to the dashboard */
(function redirectIfLoggedIn() {
  if (getActiveEmail()) {
    window.location.href = '../Client_Dashboard/index_2.html';
  }
})();

/* =========================
   ELEMENTS
========================= */

const form = document.getElementById('loginForm');
const emailInput = document.getElementById('email');
const passwordInput = document.getElementById('password');
const emailField = document.getElementById('emailField');
const passwordField = document.getElementById('passwordField');
const formMsg = document.getElementById('formMsg');
const loginBtn = document.getElementById('loginBtn');
const rememberInput = document.getElementById('remember');

/* =========================
   VALIDATION HELPERS
========================= */

function isValidEmail(value) {
  return /^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(value);
}

function clearErrors() {
  emailField.classList.remove('field-error');
  passwordField.classList.remove('field-error');
  formMsg.textContent = '';
  formMsg.className = 'form-msg';
}

/* =========================
   LOGIN
========================= */

form.addEventListener('submit', function (e) {

  e.preventDefault();
  clearErrors();

  let valid = true;

  if (!isValidEmail(emailInput.value.trim())) {
    emailField.classList.add('field-error');
    valid = false;
  }

  if (passwordInput.value.length === 0) {
    passwordField.classList.add('field-error');
    valid = false;
  }

  if (!valid) {
    formMsg.textContent = 'Please fix the errors above.';
    formMsg.classList.add('error');
    return;
  }

  const email = emailInput.value.trim().toLowerCase();
  const password = passwordInput.value;

  const users = getUsers();
  const match = users.find(function (u) {
    return u.email.toLowerCase() === email && u.password === password;
  });

  if (!match) {
    formMsg.textContent = 'No account matches that email and password.';
    formMsg.classList.add('error');
    return;
  }

  /* SIMULATE LOGIN */

  loginBtn.disabled = true;
  loginBtn.textContent = 'Logging in...';
  formMsg.textContent = '';

  setTimeout(function () {

    setSession(match.email, rememberInput.checked);

    formMsg.textContent = 'Login successful! Redirecting...';
    formMsg.classList.add('success');

    setTimeout(function () {
      window.location.href = '../Client_Dashboard/index_2.html';
    }, 500);

  }, 900);

});

/* =========================
   FORGOT PASSWORD (RESET FLOW)
========================= */

const resetModal = document.getElementById('resetModal');
const resetStep1 = document.getElementById('resetStep1');
const resetStep2 = document.getElementById('resetStep2');
const resetEmail = document.getElementById('resetEmail');
const newPassword = document.getElementById('newPassword');
const confirmPassword = document.getElementById('confirmPassword');
let resetTargetEmail = null;

function openResetModal() {
  resetStep1.style.display = 'block';
  resetStep2.style.display = 'none';
  resetEmail.value = '';
  newPassword.value = '';
  confirmPassword.value = '';
  document.getElementById('resetEmailError').parentElement.classList.remove('field-error');
  document.getElementById('newPasswordError').parentElement.classList.remove('field-error');
  document.getElementById('confirmPasswordError').parentElement.classList.remove('field-error');
  resetModal.classList.add('show');
}

function closeResetModal() {
  resetModal.classList.remove('show');
  resetTargetEmail = null;
}

document.getElementById('forgotLink').addEventListener('click', function (e) {
  e.preventDefault();
  openResetModal();
});

document.getElementById('resetCancelBtn').addEventListener('click', closeResetModal);
document.getElementById('resetBackBtn').addEventListener('click', function () {
  resetStep1.style.display = 'block';
  resetStep2.style.display = 'none';
});

resetModal.addEventListener('click', function (e) {
  if (e.target === resetModal) closeResetModal();
});

document.getElementById('resetNextBtn').addEventListener('click', function () {

  const email = resetEmail.value.trim().toLowerCase();
  const user = getUsers().find(function (u) { return u.email.toLowerCase() === email; });

  if (!user) {
    document.getElementById('resetEmailError').parentElement.classList.add('field-error');
    return;
  }

  resetTargetEmail = user.email;
  resetStep1.style.display = 'none';
  resetStep2.style.display = 'block';

});

document.getElementById('resetSubmitBtn').addEventListener('click', function () {

  const pwField = document.getElementById('newPasswordError').parentElement;
  const cfField = document.getElementById('confirmPasswordError').parentElement;

  pwField.classList.remove('field-error');
  cfField.classList.remove('field-error');

  let ok = true;

  if (newPassword.value.length < 8) {
    pwField.classList.add('field-error');
    ok = false;
  }

  if (confirmPassword.value !== newPassword.value || confirmPassword.value.length === 0) {
    cfField.classList.add('field-error');
    ok = false;
  }

  if (!ok) return;

  const users = getUsers();
  const user = users.find(function (u) { return u.email === resetTargetEmail; });

  if (user) {
    user.password = newPassword.value;
    saveUsers(users);
  }

  closeResetModal();

  formMsg.textContent = 'Password updated! You can log in with your new password.';
  formMsg.classList.add('success');

});

/* =========================
   CREATE ACCOUNT LINK
========================= */

document.getElementById('createLink').addEventListener('click', function (e) {
  e.preventDefault();
  window.location.href = '../Client_CreateAcc/index_1.html';
});

/* =========================
   COUNSELOR LOGIN
   Counselors use the same form — just pre-fill a hint.
========================= */

document.getElementById('counselorLink').addEventListener('click', function (e) {
  e.preventDefault();
  window.location.href = '../../Counselor_WEB/Counselor_Login/index.html';
});
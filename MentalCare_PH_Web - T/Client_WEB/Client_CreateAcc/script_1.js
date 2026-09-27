/* =========================
   DATABASE HELPERS
========================= */

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

/* If already logged in, skip straight to the dashboard */
(function redirectIfLoggedIn() {
  if (getActiveEmail()) {
    window.location.href = '../Client_Dashboard/index_2.html';
  }
})();

/* =========================
   ELEMENTS
========================= */

const form = document.getElementById('signupForm');
const firstName = document.getElementById('firstName');
const lastName = document.getElementById('lastName');
const email = document.getElementById('email');
const password = document.getElementById('password');
const role = document.getElementById('role');
const consent = document.getElementById('consent');

const fields = {
  firstNameField: document.getElementById('firstNameField'),
  lastNameField: document.getElementById('lastNameField'),
  emailField: document.getElementById('emailField'),
  passwordField: document.getElementById('passwordField'),
  roleField: document.getElementById('roleField'),
  consentField: document.getElementById('consentField')
};

const formMsg = document.getElementById('formMsg');
const createBtn = document.getElementById('createBtn');
const emailErrorMsg = document.getElementById('emailErrorMsg');

/* =========================
   VALIDATION
========================= */

function isValidEmail(value) {
  return /^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(value);
}

function clearErrors() {
  Object.values(fields).forEach(function (field) {
    field.classList.remove('field-error');
  });
  emailErrorMsg.textContent = 'Please enter a valid email address.';
  formMsg.textContent = '';
  formMsg.className = 'form-msg';
}

/* =========================
   FORM SUBMIT
========================= */

form.addEventListener('submit', function (e) {

  e.preventDefault();
  clearErrors();

  let valid = true;

  if (firstName.value.trim().length === 0) {
    fields.firstNameField.classList.add('field-error');
    valid = false;
  }

  if (lastName.value.trim().length === 0) {
    fields.lastNameField.classList.add('field-error');
    valid = false;
  }

  const emailValue = email.value.trim().toLowerCase();

  if (!isValidEmail(emailValue)) {
    fields.emailField.classList.add('field-error');
    valid = false;
  } else if (getUsers().some(function (u) { return u.email.toLowerCase() === emailValue; })) {
    emailErrorMsg.textContent = 'An account with this email already exists.';
    fields.emailField.classList.add('field-error');
    valid = false;
  }

  if (password.value.length < 8) {
    fields.passwordField.classList.add('field-error');
    valid = false;
  }

  if (role.value === '') {
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

  /* CREATE ACCOUNT */

  createBtn.disabled = true;
  createBtn.textContent = 'Creating account...';
  formMsg.textContent = '';

  setTimeout(function () {

    const users = getUsers();

    users.push({
      firstName: firstName.value.trim(),
      lastName: lastName.value.trim(),
      email: emailValue,
      password: password.value,
      role: role.value
    });

    saveUsers(users);

    /* Log the new user straight into a session */
    sessionStorage.setItem('mc_session', emailValue);

    formMsg.textContent = 'Account created successfully! Redirecting...';
    formMsg.classList.add('success');

    setTimeout(function () {
      window.location.href = '../Client_Dashboard/index_2.html';
    }, 600);

  }, 900);

});

/* =========================
   PRIVACY POLICY
========================= */

document.getElementById('privacyLink').addEventListener('click', function (e) {
  e.preventDefault();
  alert('Privacy Policy page goes here.\n\nIn a real deployment this would be a dedicated page explaining how mental health data is collected, stored, and protected.');
});

/* =========================
   LOGIN LINK
========================= */

document.getElementById('loginLink').addEventListener('click', function (e) {
  e.preventDefault();
  window.location.href = '../Client_Login/index.html';
});
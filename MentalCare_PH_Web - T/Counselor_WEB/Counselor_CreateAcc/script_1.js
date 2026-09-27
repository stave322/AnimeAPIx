const form = document.getElementById('requestForm');
const firstName = document.getElementById('firstName');
const lastName = document.getElementById('lastName');
const license = document.getElementById('license');
const specialization = document.getElementById('specialization');
const institution = document.getElementById('institution');
const password = document.getElementById('password');

const fields = {
  firstNameField: document.getElementById('firstNameField'),
  lastNameField: document.getElementById('lastNameField'),
  licenseField: document.getElementById('licenseField'),
  specializationField: document.getElementById('specializationField'),
  institutionField: document.getElementById('institutionField'),
  passwordField: document.getElementById('passwordField')
};

const formMsg = document.getElementById('formMsg');
const submitBtn = document.getElementById('submitBtn');

function clearErrors() {
  Object.values(fields).forEach(f => f.classList.remove('field-error'));
  formMsg.textContent = '';
  formMsg.className = 'form-msg';
}

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

  if (license.value.trim().length === 0) {
    fields.licenseField.classList.add('field-error');
    valid = false;
  }

  if (specialization.value.trim().length === 0) {
    fields.specializationField.classList.add('field-error');
    valid = false;
  }

  if (institution.value.trim().length === 0) {
    fields.institutionField.classList.add('field-error');
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
    formMsg.textContent = 'Request submitted! We will verify your license and contact you soon.';
    formMsg.classList.add('success');
    setTimeout(function () {
      window.location.href = '../Counselor_Login/index.html';
    }, 700);
  }, 1200);
});
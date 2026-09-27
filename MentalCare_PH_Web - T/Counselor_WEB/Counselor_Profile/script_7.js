const form = document.getElementById('profileForm');
const specialization = document.getElementById('specialization');
const experience = document.getElementById('experience');

const toast = document.getElementById('toast');
let toastTimer = null;
function showToast(msg) {
  toast.textContent = msg;
  toast.classList.add('show');
  clearTimeout(toastTimer);
  toastTimer = setTimeout(() => { toast.classList.remove('show'); }, 2200);
}

function setError(fieldId, hasError) {
  document.getElementById(fieldId).classList.toggle('has-error', hasError);
}

function validate() {
  let valid = true;

  if (!specialization.value.trim()) {
    setError('fieldSpecialization', true);
    valid = false;
  } else {
    setError('fieldSpecialization', false);
  }

  const expVal = Number(experience.value);
  if (experience.value === '' || isNaN(expVal) || expVal < 0 || expVal > 60) {
    setError('fieldExperience', true);
    valid = false;
  } else {
    setError('fieldExperience', false);
  }

  return valid;
}

form.addEventListener('submit', function (e) {
  e.preventDefault();
  if (!validate()) {
    showToast('Please fix the highlighted fields.');
    return;
  }
  showToast('Profile updated successfully.');
});

document.querySelectorAll('.nav-item').forEach(item => {
  item.addEventListener('click', function () {
    document.querySelectorAll('.nav-item').forEach(i => i.classList.remove('active'));
    this.classList.add('active');
  });
});
document.getElementById('saveNoteBtn').addEventListener('click', function () {
  const status = document.getElementById('saveStatus');
  status.textContent = 'Note saved.';
  setTimeout(() => { status.textContent = ''; }, 2500);
});

document.getElementById('startSessionBtn').addEventListener('click', function () {
  this.textContent = 'Session in progress';
  this.disabled = true;
});

document.querySelectorAll('.tab-item').forEach(tab => {
  tab.addEventListener('click', function () {
    document.querySelectorAll('.tab-item').forEach(t => t.classList.remove('active'));
    this.classList.add('active');
  });
});
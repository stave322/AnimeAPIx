document.getElementById('startBtn').addEventListener('click', function () {
  window.location.href = '../Counselor_Virtual/index_5.html';
});

document.getElementById('saveNoteBtn').addEventListener('click', function () {
  const saveStatus = document.getElementById('saveStatus');
  saveStatus.textContent = 'Note saved.';
  setTimeout(() => { saveStatus.textContent = ''; }, 2500);
});

document.getElementById('completeBtn').addEventListener('click', function () {
  this.textContent = '✓ Session Completed';
  this.classList.add('completed');
  this.disabled = true;
});

document.querySelectorAll('.history-item').forEach(item => {
  item.addEventListener('click', function () {
    alert(this.dataset.detail);
  });
});

document.querySelectorAll('.nav-item').forEach(item => {
  item.addEventListener('click', function () {
    document.querySelectorAll('.nav-item').forEach(i => i.classList.remove('active'));
    this.classList.add('active');
  });
});
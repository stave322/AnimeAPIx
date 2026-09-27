document.getElementById('saveSettingsBtn').addEventListener('click', function () {
  const status = document.getElementById('saveStatus');
  status.textContent = 'Settings saved.';
  setTimeout(() => { status.textContent = ''; }, 2500);
});

document.querySelectorAll('.nav-item').forEach(item => {
  item.addEventListener('click', function () {
    document.querySelectorAll('.nav-item').forEach(i => i.classList.remove('active'));
    this.classList.add('active');
  });
});
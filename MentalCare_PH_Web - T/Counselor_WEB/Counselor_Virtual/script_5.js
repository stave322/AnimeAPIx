// Audio toggle
const audioBtn = document.getElementById('audioBtn');
const audioLabel = document.getElementById('audioLabel');
let audioOn = true;
audioBtn.addEventListener('click', function () {
  audioOn = !audioOn;
  audioLabel.textContent = audioOn ? 'Audio on' : 'Audio off';
  this.style.background = audioOn ? 'var(--green)' : '#33424f';
});

// Video toggle
const videoBtn = document.getElementById('videoBtn');
const videoLabel = document.getElementById('videoLabel');
let videoOn = false;
videoBtn.addEventListener('click', function () {
  videoOn = !videoOn;
  videoLabel.textContent = videoOn ? 'Video on' : 'Video off';
  this.style.background = videoOn ? 'var(--green)' : 'var(--red)';
});

// Mute toggle
const muteBtn = document.getElementById('muteBtn');
const muteLabel = document.getElementById('muteLabel');
let muted = true;
muteBtn.addEventListener('click', function () {
  muted = !muted;
  muteLabel.textContent = muted ? 'Mute' : 'Muted';
  this.style.background = muted ? 'var(--red)' : '#33424f';
});

// End call
document.getElementById('endBtn').addEventListener('click', function () {
  alert('Call ended.');
});

// Save notes
document.getElementById('saveNotesBtn').addEventListener('click', function () {
  const status = document.getElementById('saveNotesStatus');
  status.textContent = 'Notes saved.';
  setTimeout(() => { status.textContent = ''; }, 2500);
});

document.querySelectorAll('.nav-item').forEach(item => {
  item.addEventListener('click', function () {
    document.querySelectorAll('.nav-item').forEach(i => i.classList.remove('active'));
    this.classList.add('active');
  });
});
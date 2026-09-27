// Live call timer, starting at 10 seconds like the screenshot
let seconds = 10;
const timerText = document.getElementById('timerText');

function formatTime(totalSeconds) {
  const m = Math.floor(totalSeconds / 60).toString().padStart(2, '0');
  const s = (totalSeconds % 60).toString().padStart(2, '0');
  return `${m}:${s}`;
}

setInterval(() => {
  seconds++;
  timerText.textContent = formatTime(seconds);
}, 1000);

// Audio toggle
const audioBtn = document.getElementById('audioBtn');
const audioLabel = document.getElementById('audioLabel');
let audioOn = true;
audioBtn.addEventListener('click', function () {
  audioOn = !audioOn;
  audioLabel.textContent = audioOn ? 'Audio on' : 'Audio off';
  this.style.background = audioOn ? 'var(--green)' : '#1c2b38';
});

// Video toggle
const videoBtn = document.getElementById('videoBtn');
const videoLabel = document.getElementById('videoLabel');
let videoOn = true;
videoBtn.addEventListener('click', function () {
  videoOn = !videoOn;
  videoLabel.textContent = videoOn ? 'Video on' : 'Video off';
});

// Mute toggle
const muteBtn = document.getElementById('muteBtn');
const muteLabel = document.getElementById('muteLabel');
let muted = false;
muteBtn.addEventListener('click', function () {
  muted = !muted;
  muteLabel.textContent = muted ? 'Muted' : 'Mute';
  this.style.background = muted ? 'var(--red)' : '#1c2b38';
});

// End call
document.getElementById('endBtn').addEventListener('click', function () {
  alert('Call ended.');
});
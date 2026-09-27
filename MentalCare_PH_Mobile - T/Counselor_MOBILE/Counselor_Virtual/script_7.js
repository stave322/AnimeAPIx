// Timer continues counting up from 00:14:32
let totalSeconds = 14 * 60 + 32;
const timerText = document.getElementById('timerText');

function formatTime(s) {
  const h = Math.floor(s / 3600).toString().padStart(2, '0');
  const m = Math.floor((s % 3600) / 60).toString().padStart(2, '0');
  const sec = (s % 60).toString().padStart(2, '0');
  return `${h}:${m}:${sec}`;
}

setInterval(() => {
  totalSeconds++;
  timerText.textContent = formatTime(totalSeconds);
}, 1000);

// Mic toggle
const micBtn = document.getElementById('micBtn');
let micOn = true;
micBtn.addEventListener('click', function () {
  micOn = !micOn;
  this.style.background = micOn ? 'rgba(255,255,255,0.12)' : 'var(--red)';
});

// Camera toggle
const cameraBtn = document.getElementById('cameraBtn');
let cameraOn = true;
cameraBtn.addEventListener('click', function () {
  cameraOn = !cameraOn;
  this.style.background = cameraOn ? 'rgba(255,255,255,0.12)' : 'var(--red)';
});

// End call
document.getElementById('endBtn').addEventListener('click', function () {
});

// Notes
document.getElementById('notesBtn').addEventListener('click', function () {
  alert('Opening quick session notes...');
});
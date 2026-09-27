(function(){
  // ---- Live call timer ----
  let elapsedSeconds = 14 * 60 + 32; // starts at 00:14:32 to match the screenshot
  const callTimer = document.getElementById('callTimer');

  function formatTime(totalSeconds){
    const h = Math.floor(totalSeconds / 3600);
    const m = Math.floor((totalSeconds % 3600) / 60);
    const s = totalSeconds % 60;
    return [h, m, s].map(function(v){ return String(v).padStart(2, '0'); }).join(':');
  }

  const timerInterval = setInterval(function(){
    elapsedSeconds++;
    callTimer.textContent = formatTime(elapsedSeconds);
  }, 1000);

  // ---- Toast ----
  const toast = document.getElementById('toast');
  let toastTimer = null;
  function showToast(msg){
    toast.textContent = msg;
    toast.classList.add('show');
    clearTimeout(toastTimer);
    toastTimer = setTimeout(function(){ toast.classList.remove('show'); }, 2200);
  }

  // ---- Mic / Camera toggles ----
  const micBtn = document.getElementById('micBtn');
  const camBtn = document.getElementById('camBtn');
  const selfView = document.getElementById('selfView');

  micBtn.addEventListener('click', function(){
    const muted = micBtn.classList.toggle('off');
    micBtn.textContent = muted ? '🔇' : '🎙️';
    showToast(muted ? 'Microphone muted.' : 'Microphone on.');
  });

  camBtn.addEventListener('click', function(){
    const off = camBtn.classList.toggle('off');
    camBtn.textContent = off ? '🚫' : '📷';
    selfView.classList.toggle('video-off', off);
    selfView.textContent = off ? 'Camera off' : 'You (Ms. Reyes)';
    showToast(off ? 'Camera turned off.' : 'Camera turned on.');
  });

  // ---- Chat drawer ----
  const chatBtn = document.getElementById('chatBtn');
  const chatDrawer = document.getElementById('chatDrawer');
  const chatMessages = document.getElementById('chatMessages');
  const chatInput = document.getElementById('chatInput');
  const chatSendBtn = document.getElementById('chatSendBtn');

  chatBtn.addEventListener('click', function(){
    const open = chatDrawer.classList.toggle('open');
    chatBtn.classList.toggle('chat-open', open);
  });

  function sendChat(){
    const text = chatInput.value.trim();
    if (!text) return;
    const msg = document.createElement('div');
    msg.className = 'chat-msg';
    msg.innerHTML = '<strong>You:</strong> ' + text.replace(/</g, '&lt;');
    chatMessages.appendChild(msg);
    chatMessages.scrollTop = chatMessages.scrollHeight;
    chatInput.value = '';
  }
  chatSendBtn.addEventListener('click', sendChat);
  chatInput.addEventListener('keydown', function(e){
    if (e.key === 'Enter') sendChat();
  });

  // ---- Session notes ----
  const notesArea = document.getElementById('notesArea');
  const notesMeta = document.getElementById('notesMeta');
  const saveNotesBtn = document.getElementById('saveNotesBtn');
  let savedValue = notesArea.value;

  notesArea.addEventListener('input', function(){
    if (notesArea.value !== savedValue){
      notesMeta.textContent = 'Unsaved changes';
      notesMeta.classList.remove('saved');
    }
  });

  saveNotesBtn.addEventListener('click', function(){
    savedValue = notesArea.value;
    notesMeta.textContent = 'Saved just now';
    notesMeta.classList.add('saved');
    showToast('Session notes saved.');
  });

  // ---- End call flow ----
  const endCallBtn = document.getElementById('endCallBtn');
  const endModalOverlay = document.getElementById('endModalOverlay');
  const stayBtn = document.getElementById('stayBtn');
  const confirmEndBtn = document.getElementById('confirmEndBtn');

  endCallBtn.addEventListener('click', function(){
    endModalOverlay.classList.add('open');
  });
  stayBtn.addEventListener('click', function(){
    endModalOverlay.classList.remove('open');
  });
  endModalOverlay.addEventListener('click', function(e){
    if (e.target === endModalOverlay) endModalOverlay.classList.remove('open');
  });
  confirmEndBtn.addEventListener('click', function(){
    clearInterval(timerInterval);
    savedValue = notesArea.value;
    window.location.href = '../Client_Dashboard/index_2.html';
  });
})();
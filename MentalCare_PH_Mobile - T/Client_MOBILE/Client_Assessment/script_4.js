const STORAGE_KEY = 'mentalcare_daily_assessment';

const questionSets = [
  [
    {
      text: 'Over the past two weeks, how often have you felt little interest or pleasure in doing things?',
      area: 'mood',
      options: ['Not at all', 'Several days', 'More than half the days', 'Nearly every day']
    },
    {
      text: 'How often have you felt overwhelmed by your daily tasks this week?',
      area: 'stress',
      options: ['Not at all', 'Several days', 'More than half the days', 'Nearly every day']
    },
    {
      text: 'How often have you struggled to focus on work, school, or personal tasks?',
      area: 'focus',
      options: ['Not at all', 'Several days', 'More than half the days', 'Nearly every day']
    },
    {
      text: 'How often have you felt tired or had little energy?',
      area: 'energy',
      options: ['Not at all', 'Several days', 'More than half the days', 'Nearly every day']
    },
    {
      text: 'How often have you felt emotionally connected to the people around you?',
      area: 'connection',
      options: ['Always', 'Often', 'Sometimes', 'Rarely']
    }
  ],
  [
    {
      text: 'How often have you had trouble falling or staying asleep?',
      area: 'sleep',
      options: ['Not at all', 'Several days', 'More than half the days', 'Nearly every day']
    },
    {
      text: 'How often have you felt anxious or on edge during the day?',
      area: 'stress',
      options: ['Not at all', 'Several days', 'More than half the days', 'Nearly every day']
    },
    {
      text: 'How often have you felt restless or unable to relax?',
      area: 'stress',
      options: ['Not at all', 'Several days', 'More than half the days', 'Nearly every day']
    },
    {
      text: 'How often have you had trouble making decisions or planning your day?',
      area: 'focus',
      options: ['Not at all', 'Several days', 'More than half the days', 'Nearly every day']
    },
    {
      text: 'How often have you felt like you needed support from someone you trust?',
      area: 'connection',
      options: ['Rarely', 'Sometimes', 'Often', 'Very often']
    }
  ],
  [
    {
      text: 'How often have you felt down, depressed, or hopeless?',
      area: 'mood',
      options: ['Not at all', 'Several days', 'More than half the days', 'Nearly every day']
    },
    {
      text: 'How often have you lost motivation to do the things you normally enjoy?',
      area: 'mood',
      options: ['Not at all', 'Several days', 'More than half the days', 'Nearly every day']
    },
    {
      text: 'How often have you felt isolated from friends or family?',
      area: 'connection',
      options: ['Not at all', 'Several days', 'More than half the days', 'Nearly every day']
    },
    {
      text: 'How often have you had poor appetite or overeating?',
      area: 'energy',
      options: ['Not at all', 'Several days', 'More than half the days', 'Nearly every day']
    },
    {
      text: 'How often have you found it hard to recover after a stressful event?',
      area: 'stress',
      options: ['Not at all', 'Several days', 'More than half the days', 'Nearly every day']
    }
  ],
  [
    {
      text: 'How often have you felt nervous or worried about things outside your control?',
      area: 'stress',
      options: ['Not at all', 'Several days', 'More than half the days', 'Nearly every day']
    },
    {
      text: 'How often have you been able to calm yourself down when things felt intense?',
      area: 'mood',
      options: ['Always', 'Often', 'Sometimes', 'Rarely']
    },
    {
      text: 'How often have you struggled to maintain a healthy daily routine?',
      area: 'energy',
      options: ['Not at all', 'Several days', 'More than half the days', 'Nearly every day']
    },
    {
      text: 'How often have you felt hopeful about improving your situation?',
      area: 'mood',
      options: ['Always', 'Often', 'Sometimes', 'Rarely']
    },
    {
      text: 'How often have you reached out to someone for support or encouragement?',
      area: 'connection',
      options: ['Always', 'Often', 'Sometimes', 'Rarely']
    }
  ]
];

const questionCount = document.getElementById('questionCount');
const progressFill = document.getElementById('progressFill');
const questionText = document.getElementById('questionText');
const optionsContainer = document.getElementById('optionsContainer');
const backBtn = document.getElementById('backBtn');
const nextBtn = document.getElementById('nextBtn');
const assessmentFlow = document.getElementById('assessmentFlow');
const completeState = document.getElementById('completeState');
const resultSummary = document.getElementById('resultSummary');
const restartBtn = document.getElementById('restartBtn');

let questions = [];
let currentIndex = 0;
let answers = [];
let assessmentState = null;

function safeRead(key, fallback) {
  try {
    const stored = localStorage.getItem(key);
    return stored ? JSON.parse(stored) : fallback;
  } catch (error) {
    return fallback;
  }
}

function safeWrite(key, value) {
  try {
    localStorage.setItem(key, JSON.stringify(value));
  } catch (error) {
    console.warn('Unable to persist assessment state.', error);
  }
}

function formatDateKey(date) {
  const year = date.getFullYear();
  const month = String(date.getMonth() + 1).padStart(2, '0');
  const day = String(date.getDate()).padStart(2, '0');
  return `${year}-${month}-${day}`;
}

function getTodayQuestionSet() {
  const today = new Date();
  const daySeed = (today.getFullYear() * 10000 + (today.getMonth() + 1) * 100 + today.getDate()) % questionSets.length;
  return questionSets[daySeed];
}

function loadAssessmentState() {
  const todayKey = formatDateKey(new Date());
  const saved = safeRead(STORAGE_KEY, null);

  if (saved && saved.dateKey === todayKey) {
    return saved;
  }

  const dailyQuestions = getTodayQuestionSet();
  return {
    dateKey: todayKey,
    questions: dailyQuestions,
    answers: Array(dailyQuestions.length).fill(null),
    currentIndex: 0,
    completed: false,
    summary: null,
    totalQuestions: dailyQuestions.length
  };
}

function persistAssessmentState() {
  safeWrite(STORAGE_KEY, assessmentState);
}

function buildSummary(questionsList, answersList) {
  const categoryTotals = {
    mood: 0,
    stress: 0,
    focus: 0,
    energy: 0,
    connection: 0,
    sleep: 0
  };

  questionsList.forEach((question, index) => {
    const answerScore = answersList[index] ?? 0;
    categoryTotals[question.area] += answerScore;
  });

  const dominantArea = Object.entries(categoryTotals).sort((a, b) => b[1] - a[1])[0];
  const totalScore = answersList.reduce((sum, answer) => sum + (answer ?? 0), 0);

  let title = 'You seem steady';
  let comment = 'Your answers suggest you are managing things fairly well. Keep checking in so you can notice small shifts early.';

  if (dominantArea[1] >= 6 || totalScore >= 12) {
    title = 'A little support may help';
    comment = 'Your responses suggest that stress, focus, or mood may be heavier than usual. Gentle support, rest, and grounding routine could help right now.';
  }

  if (dominantArea[1] >= 9 || totalScore >= 16) {
    title = 'You may need extra care';
    comment = 'Your check-in shows signs that things may feel quite demanding. Please consider taking it one step at a time and reaching out to someone you trust.';
  }

  return {
    title,
    comment,
    dominantArea: dominantArea[0],
    totalScore,
    recommendations: getRecommendations(dominantArea[0], totalScore)
  };
}

function getRecommendations(area, totalScore) {
  const resourceMap = {
    mood: [
      { type: 'Article', title: '5 small habits to lift your mood today', note: 'A quick read on routines that can make a real difference.' },
      { type: 'Video', title: 'A 5-minute guided breathing exercise', note: 'Helpful if you want a calm reset right away.' }
    ],
    stress: [
      { type: 'Article', title: 'Grounding techniques for stress relief', note: 'Simple practices to help you settle when overwhelmed.' },
      { type: 'Video', title: 'Gentle body scan for anxiety relief', note: 'A calming, low-pressure option you can do in 5 minutes.' }
    ],
    focus: [
      { type: 'Article', title: 'How to reset your focus when everything feels noisy', note: 'A practical guide to break big tasks into manageable steps.' },
      { type: 'Video', title: 'A 10-minute focus reset routine', note: 'Perfect for busy days when concentration feels scattered.' }
    ],
    energy: [
      { type: 'Article', title: 'Energy boosters that actually work', note: 'Helpful ideas for sleep, meals, and movement.' },
      { type: 'Video', title: 'Quick stretch and wake-up routine', note: 'A simple way to recharge when you feel drained.' }
    ],
    sleep: [
      { type: 'Article', title: 'Sleep reset routine for better rest', note: 'Easy steps to create a calmer evening wind-down.' },
      { type: 'Video', title: 'Guided wind-down for better sleep', note: 'A relaxing video to help your mind slow down.' }
    ],
    connection: [
      { type: 'Article', title: 'How to reach out when you feel isolated', note: 'Small, supportive ways to reconnect with people.' },
      { type: 'Video', title: 'A quick check-in with a trusted person', note: 'A gentle reminder that support is available.' }
    ]
  };

  const recommendations = resourceMap[area] || resourceMap.mood;
  if (totalScore >= 10) {
    return [
      ...recommendations,
      { type: 'Reminder', title: 'Book a counseling follow-up', note: 'A quick session may help you work through what felt most difficult.' }
    ];
  }

  return recommendations;
}

function renderQuestion() {
  const question = questions[currentIndex];
  questionCount.textContent = `QUESTION ${currentIndex + 1} OF ${questions.length}`;
  progressFill.style.width = `${((currentIndex + 1) / questions.length) * 100}%`;
  questionText.textContent = question.text;

  optionsContainer.innerHTML = '';
  question.options.forEach((option, optionIndex) => {
    const label = document.createElement('label');
    label.className = 'option' + (answers[currentIndex] === optionIndex ? ' selected' : '');

    const radio = document.createElement('input');
    radio.type = 'radio';
    radio.name = 'option';
    radio.value = optionIndex;
    radio.checked = answers[currentIndex] === optionIndex;

    radio.addEventListener('change', () => {
      answers[currentIndex] = Number(optionIndex);
      assessmentState.answers = answers;
      persistAssessmentState();
      renderQuestion();
    });

    const span = document.createElement('span');
    span.className = 'option-label';
    span.textContent = option;

    label.appendChild(radio);
    label.appendChild(span);
    optionsContainer.appendChild(label);
  });

  backBtn.disabled = currentIndex === 0;
  nextBtn.disabled = answers[currentIndex] === null || answers[currentIndex] === undefined;
  nextBtn.textContent = currentIndex === questions.length - 1 ? 'Finish' : 'Next';
}

function showCompletionState() {
  const summary = buildSummary(questions, answers);
  assessmentState.completed = true;
  assessmentState.summary = summary;
  assessmentState.answers = answers;
  assessmentState.totalQuestions = questions.length;
  persistAssessmentState();

  assessmentFlow.style.display = 'none';
  completeState.classList.add('show');

  resultSummary.innerHTML = `
    <div class="result-badge">${summary.title}</div>
    <h3>Here is a quick check-in summary</h3>
    <p class="result-comment">${summary.comment}</p>
    <div class="recommendation-list">
      ${summary.recommendations
        .map(
          recommendation => `
            <div class="recommendation-item">
              <span class="recommendation-type">${recommendation.type}</span>
              <div>
                <strong>${recommendation.title}</strong>
                <small>${recommendation.note}</small>
              </div>
            </div>
          `
        )
        .join('')}
    </div>
  `;
}

function resetAssessment() {
  const todayKey = formatDateKey(new Date());
  const dailyQuestions = getTodayQuestionSet();
  assessmentState = {
    dateKey: todayKey,
    questions: dailyQuestions,
    answers: Array(dailyQuestions.length).fill(null),
    currentIndex: 0,
    completed: false,
    summary: null,
    totalQuestions: dailyQuestions.length
  };

  questions = assessmentState.questions;
  answers = assessmentState.answers;
  currentIndex = assessmentState.currentIndex;
  persistAssessmentState();

  completeState.classList.remove('show');
  assessmentFlow.style.display = 'block';
  resultSummary.innerHTML = '';
  renderQuestion();
}

function bindControls() {
  backBtn.addEventListener('click', () => {
    if (currentIndex > 0) {
      currentIndex -= 1;
      renderQuestion();
    }
  });

  nextBtn.addEventListener('click', () => {
    if (answers[currentIndex] === null || answers[currentIndex] === undefined) {
      return;
    }

    if (currentIndex < questions.length - 1) {
      currentIndex += 1;
      renderQuestion();
    } else {
      showCompletionState();
    }
  });

  restartBtn.addEventListener('click', () => {
    resetAssessment();
  });

  document.querySelectorAll('.tab-item').forEach(tab => {
    tab.addEventListener('click', function () {
      document.querySelectorAll('.tab-item').forEach(t => t.classList.remove('active'));
      this.classList.add('active');
    });
  });
}

function initAssessment() {
  assessmentState = loadAssessmentState();
  questions = assessmentState.questions;
  answers = assessmentState.answers;
  currentIndex = assessmentState.currentIndex;

  if (assessmentState.completed) {
    showCompletionState();
  } else {
    renderQuestion();
  }

  bindControls();
}

initAssessment();
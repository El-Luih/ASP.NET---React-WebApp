import { useEffect, useState } from 'react'
import type { FormEvent } from 'react'
import '../App.css'

type Category = { id: number; name: string }
type Question = {
  id: number
  text: string
  optionA: string
  optionB: string
  optionC: string
  optionD: string
}
type ScoreResult = { correct: number; total: number; playerName: string }
type LeaderboardEntry = {
  playerName: string
  correct: number
  total: number
  createdAt: string
}

// Keeping the API base in one place makes local proxying and deployed URLs easy to change.
const API_URL = import.meta.env.VITE_API_URL ?? '/api'
const options = ['A', 'B', 'C', 'D'] as const
type Option = (typeof options)[number]

function App() {
  // The app is a single React page, but these steps give the player a clear multi-page flow.
  const [step, setStep] = useState<'name' | 'category' | 'quiz' | 'results'>('name')
  const [playerName, setPlayerName] = useState('')
  const [nameInput, setNameInput] = useState('')
  const [categories, setCategories] = useState<Category[]>([])
  const [categoryId, setCategoryId] = useState<number | null>(null)
  const [questions, setQuestions] = useState<Question[]>([])
  const [answers, setAnswers] = useState<Record<number, Option>>({})
  const [score, setScore] = useState<ScoreResult | null>(null)
  const [leaderboard, setLeaderboard] = useState<LeaderboardEntry[]>([])
  const [loading, setLoading] = useState(false)
  const [error, setError] = useState<string | null>(null)

  useEffect(() => {
    // Categories are loaded once when the app opens so the selection screen uses database data.
    fetch(`${API_URL}/categories`)
      .then(async response => {
        if (!response.ok) throw new Error('Categories could not be loaded.')
        setCategories((await response.json()) as Category[])
      })
      .catch(err => setError(err.message))
  }, [])

  async function verifyName(event: FormEvent) {
    // Name verification happens before the player is allowed to start a quiz.
    event.preventDefault()
    const name = nameInput.trim()
    if (!name) return setError('Enter a username to continue.')
    setLoading(true)
    setError(null)
    try {
      const response = await fetch(`${API_URL}/players/verify-name`, {
        method: 'POST', headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ playerName: name }),
      })
      const result = (await response.json()) as { available: boolean; message?: string }
      if (!result.available) throw new Error(result.message ?? 'That username is already taken.')
      setPlayerName(name)
      setStep('category')
    } catch (err) { setError(err instanceof Error ? err.message : 'Could not verify username.') }
    finally { setLoading(false) }
  }

  async function startQuiz(event: FormEvent) {
    // The backend chooses the questions so the client never controls the quiz contents.
    event.preventDefault()
    if (!categoryId) return setError('Choose a category to begin.')
    setLoading(true)
    setError(null)
    try {
      const response = await fetch(`${API_URL}/questions?categoryId=${categoryId}&count=7`)
      if (!response.ok) throw new Error('Questions could not be loaded.')
      setQuestions((await response.json()) as Question[])
      setAnswers({})
      setStep('quiz')
    } catch (err) { setError(err instanceof Error ? err.message : 'Could not start quiz.') }
    finally { setLoading(false) }
  }

  async function submitQuiz(event: FormEvent) {
    // Send question IDs and selected options; the backend remains responsible for grading.
    event.preventDefault()
    if (!categoryId || Object.keys(answers).length !== questions.length) {
      return setError('Answer every question before submitting.')
    }
    setLoading(true)
    setError(null)
    try {
      const response = await fetch(`${API_URL}/scores`, {
        method: 'POST', headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({
          playerName, categoryId,
          answers: questions.map(question => ({ questionId: question.id, selectedOption: answers[question.id] })),
        }),
      })
      if (!response.ok) throw new Error(await response.text() || 'Score could not be submitted.')
      const result = (await response.json()) as ScoreResult
      // Fetch the leaderboard after saving so the new score appears immediately.
      const leaderboardResponse = await fetch(`${API_URL}/scores?categoryId=${categoryId}&top=10`)
      if (!leaderboardResponse.ok) throw new Error('Leaderboard could not be loaded.')
      setScore(result)
      setLeaderboard((await leaderboardResponse.json()) as LeaderboardEntry[])
      setStep('results')
    } catch (err) { setError(err instanceof Error ? err.message : 'Could not submit quiz.') }
    finally { setLoading(false) }
  }

  function resetGame() {
    setNameInput(''); setPlayerName(''); setCategoryId(null); setQuestions([]); setAnswers({}); setScore(null); setError(null); setStep('name')
  }

  const selectedCategory = categories.find(category => category.id === categoryId)

  return (
    <main className="app-shell">
      {/* Each step renders only the controls needed for that part of the experience. */}
      <header className="topbar"><a className="brand" href="/" aria-label="InsTrivia home"><img src="/ins-trivia-mark.svg" alt="" /> <span>InsTrivia</span></a><span className="status">{step === 'quiz' ? 'QUIZ IN PROGRESS' : 'TRIVIA NIGHT'}</span></header>
      <section className="content">
        {step === 'name' && <section className="welcome-panel"><p className="eyebrow">A little knowledge goes a long way</p><h1>Play with your<br /><em>curiosity.</em></h1><p className="intro">Choose a name, pick a category, and see how far you can go.</p><form onSubmit={verifyName} className="entry-form"><label htmlFor="username">Your username</label><input id="username" value={nameInput} onChange={event => setNameInput(event.target.value)} placeholder="e.g. Ada Lovelace" autoComplete="off" /><button type="submit" disabled={loading}>{loading ? 'Checking...' : 'Continue'} <span>→</span></button></form></section>}
        {step === 'category' && <section className="setup-panel"><p className="eyebrow">Welcome, {playerName}</p><h1>What are you<br /><em>curious about?</em></h1><form onSubmit={startQuiz} className="entry-form"><label htmlFor="category">Select a category</label><select id="category" value={categoryId ?? ''} onChange={event => setCategoryId(Number(event.target.value) || null)}><option value="">Choose one...</option>{categories.map(category => <option key={category.id} value={category.id}>{category.name}</option>)}</select><button type="submit" disabled={loading}>{loading ? 'Loading...' : 'Start quiz'} <span>→</span></button></form><button className="text-button" onClick={resetGame}>Use a different name</button></section>}
        {step === 'quiz' && <section className="quiz-panel"><div className="quiz-heading"><div><p className="eyebrow">{selectedCategory?.name ?? 'Quiz'} · {playerName}</p><h1>Trust your<br /><em>first thought.</em></h1></div><span className="question-count">{Object.keys(answers).length}/{questions.length} answered</span></div><form onSubmit={submitQuiz} className="questions">{questions.map((question, index) => <fieldset key={question.id}><legend><span>{String(index + 1).padStart(2, '0')}</span>{question.text}</legend><div className="answer-grid">{options.map(option => <label key={option} className={answers[question.id] === option ? 'answer selected' : 'answer'}><input type="radio" name={`question-${question.id}`} value={option} checked={answers[question.id] === option} onChange={() => setAnswers(current => ({ ...current, [question.id]: option }))} /><b>{option}</b><span>{question[`option${option}` as keyof Question] as string}</span></label>)}</div></fieldset>)}<button className="submit-button" type="submit" disabled={loading}>{loading ? 'Submitting...' : 'Submit answers'} <span>→</span></button></form></section>}
        {step === 'results' && score && <section className="results-panel"><div className="score-hero"><p className="eyebrow">Quiz complete · {score.playerName}</p><h1>You scored<br /><em>{score.correct} / {score.total}</em></h1><p>{score.correct === score.total ? 'Perfect round.' : 'A strong showing. Ready for another round?'}</p><button onClick={resetGame}>Play again <span>↗</span></button></div><div className="leaderboard"><div className="leaderboard-title"><div><p className="eyebrow">The hall of fame</p><h2>Leaderboard</h2></div><span>{selectedCategory?.name}</span></div>{leaderboard.length === 0 ? <p className="empty">No scores yet. Be the first.</p> : <ol>{leaderboard.map((entry, index) => <li key={`${entry.playerName}-${entry.createdAt}`} className={entry.playerName === score.playerName ? 'current' : ''}><span className="rank">{String(index + 1).padStart(2, '0')}</span><strong>{entry.playerName}</strong><span className="leader-score">{entry.correct}<small>/{entry.total}</small></span></li>)}</ol>}</div></section>}
        {error && <p className="error" role="alert">{error}</p>}
      </section>
      <footer><span>Luis Maradiaga · INS · 2026</span><span>Think boldly. Play lightly.</span></footer>
    </main>
  )
}

export default App
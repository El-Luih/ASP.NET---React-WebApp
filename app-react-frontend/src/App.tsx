import { useEffect, useState } from 'react'
import './App.css'

type WeatherForecast = {
  date: string
  temperatureC: number
  temperatureF: number
  summary: string
}

function App() {
  const [forecasts, setForecasts] = useState<WeatherForecast[]>([])
  const [loading, setLoading] = useState(true)
  const [error, setError] = useState<string | null>(null)

  useEffect(() => {
    fetch('/WeatherForecast')
      .then(async response => {
        if (!response.ok) {
          throw new Error(`It didn't work, tough luck: ${response.status}`)
        }
        const data = (await response.json()) as WeatherForecast[]
        setForecasts(data)
      })
      .catch(err => setError(err.message))
      .finally(() => setLoading(false))
  }, [])

  if (loading) {
    return <p>Fucking wait, gosh...</p>
  }

  if (error) {
    return <p>About that...<br></br>{error}</p>
  }

  return (
    <main style={{ padding: 24 }}>
      <h1>Weather Forecast</h1>
      <table>
        <thead>
          <tr>
            <th>Date</th>
            <th>Temp (C)</th>
            <th>Temp (F)</th>
            <th>Summary</th>
          </tr>
        </thead>
        <tbody>
          {forecasts.map(item => (
            <tr key={item.date}>
              <td>{item.date}</td>
              <td>{item.temperatureC}</td>
              <td>{item.temperatureF}</td>
              <td>{item.summary}</td>
            </tr>
          ))}
        </tbody>
      </table>
    </main>
  )
}

export default App
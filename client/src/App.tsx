import { useWeather } from './useWeather'
import { WeatherCard } from './WeatherCard'
import './App.css'


function App() {
  const { weathers, loading, errorMessage } = useWeather()

  if ( loading ) {
    return <p>Loading</p>
  }
  if ( errorMessage ) {
      return <p>{ errorMessage }</p>
  }
  return (
    <>
      <section id="center">
        <div className="hero">
          <h1>AppsHub</h1>
        </div>
        <h2>Weather forecast</h2>
        {weathers.map( ( weather ) => 
          <WeatherCard key={ weather.date } weather={ weather }/>
        )}
      </section>
    </>
  )
}

export default App
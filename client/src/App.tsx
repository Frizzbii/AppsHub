import { useWeather } from './useWeather'
import { WeatherCard } from './WeatherCard'
import type { ReactNode } from 'react';
import './App.css'


function App() {
  const { weathers, loading, errorMessage, fetchWeather } = useWeather()

  let weatherDisplay : ReactNode;

  if ( loading && !weathers ) { weatherDisplay = <p>Loading</p> }
  else if ( errorMessage ) { weatherDisplay = <p>{ errorMessage }</p> }
  else {
    weatherDisplay = weathers.map( ( weather ) =>
      <WeatherCard key={ weather.date } weather={ weather }/> 
    )
  }
  return (
    <>
      <section id="center">
        <div className="hero">
          <h1>AppsHub</h1>
        </div>
        <div>
          <h2>Weather forecast</h2>
          <button onClick={ fetchWeather } disabled={ loading }>Refresh weather</button>
          { weatherDisplay }
        </div>
      </section>
    </>
  )
}

export default App
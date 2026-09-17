import { useWeather } from './useWeather'
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
        </div>
        <div>
          <h1>Weather forecast</h1>
          
          {weathers.map( ( weather ) => 
            <p key={ weather.date }>{ weather.date }, { weather.temperatureC }, { weather.temperatureF }, { weather.summary }</p>
          ) }
        </div>
      </section>
    </>
  )
}

export default App
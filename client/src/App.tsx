import { useState, useEffect } from 'react'
import './App.css'


type Weather = {
  date: string;
  temperatureC: number;
  temperatureF: number;
  summary: string | null;
}


function App() {
  const [ weathers, setWeathers ] = useState<Weather[]>( [] );


  useEffect(  () => {
    const fetchWeather = async() => {
      const response = await fetch( '/api/weatherforecast' );
      const data = await response.json();

      setWeathers( data );
    };

    fetchWeather();
  }, [] );
  

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
import { useState, useEffect } from 'react'


type Weather = {
  date: string;
  temperatureC: number;
  temperatureF: number;
  summary: string | null;
}

type UseWeatherResult = {
  weathers: Weather[];
  loading: boolean;
  errorMessage: string | null;
};


function useWeather(): UseWeatherResult {
    const [ weathers, setWeathers ] = useState<Weather[]>( [] );
    const [ loading, setLoading ] = useState<boolean>( true )
    const [ errorMessage, setErrorMessage ] = useState<string | null>( null )


    useEffect(  () => {
        const fetchWeather = async() => {
        try {
            const response = await fetch( '/api/weatherforecast' );
            
            if ( !response.ok ) {
            throw new Error( response.status.toString() );
            }

            const data = await response.json();

            setWeathers( data );
        } 
        
        catch ( error ) {
            if (error instanceof Error) {
            setErrorMessage( error.message )
            console.error( error.message )
            }
            else {
            setErrorMessage( 'Erreur inconnue.' )
            console.error( 'Erreur inconnue.' )
            }
        }
        
        finally {
            setLoading( false );
        }
        
        };

        fetchWeather();
    }, [] );

    return { weathers, loading, errorMessage }
}


export { useWeather }
import { useState, useEffect } from 'react'
import type { Weather } from './types';


type UseWeatherResult = {
  weathers: Weather[];
  loading: boolean;
  errorMessage: string | null;
  fetchWeather : () => Promise<void>;
};


function useWeather(): UseWeatherResult {
    const [ weathers, setWeathers ] = useState<Weather[]>( [] );
    const [ loading, setLoading ] = useState<boolean>( true )
    const [ errorMessage, setErrorMessage ] = useState<string | null>( null )

    const fetchWeather = async() => {
        setLoading( true );
        setErrorMessage( null )

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
                setErrorMessage( error.message );
                console.error( error.message );
            }
            else {
                setErrorMessage( 'Erreur inconnue.' );
                console.error( 'Erreur inconnue.' );
            }
        }
            
        finally {
            setLoading( false );
        }
    };

    useEffect(  () => {
        fetchWeather();
    }, [] );

    return { weathers, loading, errorMessage, fetchWeather }
}

export { useWeather }
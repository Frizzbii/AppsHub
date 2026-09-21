import type { Weather } from "./types";

type WeatherCardProps = {
	weather: Weather;
};

function WeatherCard({ weather }: WeatherCardProps) {
	return (
		<p>
			{weather.date}, {weather.temperatureC}, {weather.temperatureF},{" "}
			{weather.summary}
		</p>
	);
}

export { WeatherCard };

import { useWeather } from "./useWeather";
import { WeatherCard } from "./WeatherCard";
import { useState } from "react";
import type { ReactNode } from "react";
import "./App.css";

function App() {
	const { weathers, loading, errorMessage, fetchWeather } = useWeather();
	const [filter, setFilter] = useState<string>("");

	let weatherDisplay: ReactNode;

	if (loading && weathers.length === 0) {
		weatherDisplay = <p>Loading</p>;
	} else if (errorMessage) {
		weatherDisplay = <p>{errorMessage}</p>;
	} else {
		const filteredWeathers = weathers.filter(
			(weather) =>
				filter === "" ||
				weather.summary?.toLowerCase().includes(filter.toLowerCase()),
		);

		weatherDisplay = filteredWeathers.map((weather) => (
			<WeatherCard key={weather.date} weather={weather} />
		));
	}
	return (
		<>
			<section id="center">
				<div className="hero">
					<h1>AppsHub</h1>
				</div>
				<div>
					<h2>Weather forecast</h2>
					<input
						name="filter"
						value={filter}
						onChange={(e) => setFilter(e.target.value)}
					></input>
					<br />
					<button onClick={fetchWeather} disabled={loading}>
						Refresh weather
					</button>
					{weatherDisplay}
				</div>
			</section>
		</>
	);
}

export default App;

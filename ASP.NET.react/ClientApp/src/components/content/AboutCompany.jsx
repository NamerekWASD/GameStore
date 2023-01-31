import React, { useEffect, useState } from 'react';

const AboutCompany = (props) => {
    const [forecast, setForecast] = useState({ forecasts: [], loading: true });

    async function populateWeatherData() {
        await fetch('weatherforecast')
            .then(response => {
                return response.json();
            })
            .then(data => {
                setForecast({ forecasts: data, loading: false })
            })
    }
    useEffect(() => {
        if (forecast.loading) {
            populateWeatherData()
        }
    })

    function renderForecastsTable(forecasts) {
        return (
            <main>
                <table className='table table-striped' aria-labelledby="tabelLabel">
                    <thead>
                        <tr>
                            <th>Date</th>
                            <th>Temp. (C)</th>
                            <th>Temp. (F)</th>
                            <th>Summary</th>
                        </tr>
                    </thead>
                    <tbody>
                        {forecasts.map(forecast =>
                            <tr key={forecast.date}>
                                <td>{forecast.date}</td>
                                <td>{forecast.temperatureC}</td>
                                <td>{forecast.temperatureF}</td>
                                <td>{forecast.summary}</td>
                            </tr>
                        )}
                    </tbody>
                </table>
            </main>
        );
    }
    let contents = forecast.loading
        ? <p><em>Loading...</em></p>
        : renderForecastsTable(forecast.forecasts);

    return (
        <div>
            <h1 id="tabelLabel" >Weather forecast</h1>
            <p>This component demonstrates fetching data from the server.</p>
            {contents}
        </div>
    );
}

export default AboutCompany;

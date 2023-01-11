﻿using FarmAdvisor.DataAccess.MSSQL.Entities;
using System.Net.Http.Json;
namespace FarmAdvisor.Business
{

    public class GetWeatherForecast:IDisposable
    {
        private readonly HttpClient _httpCLient;

        public GetWeatherForecast()
        {
            _httpCLient = new HttpClient();
        }

        public void Dispose()
        {
            ((IDisposable)_httpCLient).Dispose();
        }

        public async Task<WeatherForecast?> GetLocationForecast(Sensor sensor)
        {
            _httpCLient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/108.0.0.0 Safari/537.36");
            try
            {
                WeatherForecast response = await _httpCLient.GetFromJsonAsync<WeatherForecast>($"https://api.met.no/weatherapi/locationforecast/2.0/complete?lat={sensor.Lat.ToString()}&lon={sensor.Long.ToString()}&altitude={sensor.Field.Alt.ToString()}");
                return response;
            }
            catch (HttpRequestException)
            {
                return null;
            }
        }


    }
}

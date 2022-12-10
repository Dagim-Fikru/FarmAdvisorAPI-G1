using FarmAdvisor.DataAccess.AzureTableStorage.services;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
[assembly: FunctionsStartup(typeof(FarmAdvisor.HttpFunctions.HttpFunctionStartup))]

namespace FarmAdvisor.HttpFunctions
{
    public class HttpFunctionStartup : FunctionsStartup
    {
        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<ISensorDataStorageService, SensorDataStorageService>();
            services.AddScoped<IWeatherStorageService, WeatherStorageService>();
        }


        public override void Configure(IFunctionsHostBuilder builder)
            => ConfigureServices(builder.Services);

    }
}
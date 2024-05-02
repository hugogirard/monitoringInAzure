using Azure.Messaging.ServiceBus;
using Azure.Messaging.ServiceBus.Administration;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using WeatherApi.Repository;

var builder = WebApplication.CreateBuilder(args);
builder.Services.RegisterServices(builder.Configuration);

//var srvBusClient = new ServiceBusClient("");
//var topicSender = srvBusClient.CreateSender("weather");
//await topicSender.SendMessageAsync(new ServiceBusMessage(JsonSerializer.Serialize(weather)));

builder.Logging.AddFile("C://Logs/WeatherApi-{Date}.txt");
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

var logger = app.Logger;

app.UseHttpsRedirection();


app.MapGet("/weather/byCityName/{cityName}", async ([FromServices] IWeatherRepository repository,  string cityName) =>
{
    var weather = await repository.GetWeatherByCityName(cityName);

    if (weather is null)
    {
        logger.LogWarning($"Weather for {cityName} not found");
        return Results.NotFound();
    }

    logger.LogInformation($"Retrieved weather for {cityName}");
    logger.LogInformation($"Weather is {weather.TemperatureC}");

    return Results.Ok(weather);
  
})
.WithName("GetWeatherForecastByCityName")
.WithOpenApi();

app.MapGet("/weather/byCityId/{cityId}", async ([FromServices] IWeatherRepository repository, int cityId) =>
{
    var weather = await repository.GetWeatherByCityId(cityId);

    if (weather is null)
    {
        logger.LogWarning($"Weather for city ID {cityId} not found");
        return Results.NotFound();
    }

    logger.LogInformation($"Retrieved weather for city ID {cityId}");
    logger.LogInformation($"Weather is {weather.TemperatureC}");
    
    return Results.Ok(weather);
})
.WithName("GetWeatherForecastByCityID")
.WithOpenApi();

app.MapGet("/weather/cities", async ([FromServices] IWeatherRepository repository) =>
{
    logger.LogInformation("Retrieved all cities");
    var weatherData = await repository.GetWeatherData();
    return Results.Ok(weatherData);
})
.WithName("GetWeatherCities")
.WithOpenApi(); ;

app.MapPut("/weather/updateWeather/{cityId}", ([FromServices] IWeatherRepository repository, int cityId, [FromBody] WeatherForecast updatedWeather) =>
{

    var weather = repository.UpdateWeather(cityId, updatedWeather);

    if (weather is null)
    {
        logger.LogWarning($"Weather for city ID {cityId} not found");
        return Results.NotFound();
    }

    logger.LogInformation($"Updated weather for city ID {cityId}");
    return Results.Ok();       
})
.WithName("UpdateWeatherForecast")
.WithOpenApi();

app.Run();

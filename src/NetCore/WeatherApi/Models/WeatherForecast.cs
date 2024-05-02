namespace WeatherApi.Models;

public record WeatherForecast(int CityId,string CityName, int TemperatureC, string? Summary);

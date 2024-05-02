using Azure.Core.Serialization;
using System.Text.Json;
using WeatherApi.Models;

namespace WeatherApi.Repository;

public class WeatherRepository : IWeatherRepository
{
    private readonly Dictionary<int, WeatherForecast> _weatherData = new()
    {
        { 1, new WeatherForecast(1, "Seattle", 10, "Rainy") },
        { 2, new WeatherForecast(2, "Los Angeles", 25, "Sunny") },
        { 3, new WeatherForecast(3, "New York", 15, "Cloudy") },
        { 4, new WeatherForecast(4, "Chicago", 20, "Windy") },
        { 5, new WeatherForecast(5, "Houston", 30, "Hot") },
        { 6, new WeatherForecast(6, "Phoenix", 35, "Sunny") },
        { 7, new WeatherForecast(7, "Philadelphia", 18, "Rainy") },
        { 8, new WeatherForecast(8, "San Antonio", 28, "Hot") },
        { 9, new WeatherForecast(9, "San Diego", 22, "Sunny") },
        { 10, new WeatherForecast(10, "Dallas", 32, "Hot") }
    };
    private readonly Task<RedisConnection> _redisConnectionFactory;
    private readonly ILogger<WeatherRepository> _logger;
    private RedisConnection _redisConnection;

    public WeatherRepository(Task<RedisConnection> redisConnectionFactory,ILogger<WeatherRepository> logger)
    {
        _redisConnectionFactory = redisConnectionFactory;
        _logger = logger;
    }

    public async Task<WeatherForecast> GetWeatherByCityName(string cityName)
    {
        WeatherForecast weatherForecast = await GetCachedValueByKeyAsync(cityName);

        if (weatherForecast == null)
        {
            foreach (var weather in _weatherData.Values)
            {
                if (weather.CityName.Equals(cityName, StringComparison.OrdinalIgnoreCase))
                {
                    weatherForecast = weather;
                    await CacheValueAsync(cityName, weatherForecast);
                    break;
                }
            }                   
        }

        return weatherForecast;
    }

    public async Task<WeatherForecast> GetWeatherByCityId(int cityId)
    {
        WeatherForecast weatherForecast = await GetCachedValueByKeyAsync(cityId.ToString());

        if (weatherForecast == null)
        {
            if (_weatherData.TryGetValue(cityId, out var weather))
            {
                weatherForecast = weather;
                await CacheValueAsync(cityId.ToString(), weatherForecast);
            }
        }

        return weatherForecast;
    }

    public async Task<IEnumerable<WeatherForecast>> GetWeatherData()
    {
        return _weatherData.Values;
    }

    public async Task<WeatherForecast> UpdateWeather(int cityId, WeatherForecast updatedWeather)
    {
        if (_weatherData.ContainsKey(cityId))
        {
            _weatherData[cityId] = updatedWeather;

            // Cache the new value
            await CacheValueAsync(cityId.ToString(), updatedWeather);

            return updatedWeather;
        }

        return null;
    }

    private async Task<WeatherForecast> GetCachedValueByKeyAsync(string key)
    {
        try
        {
            _redisConnection = await _redisConnectionFactory;

            var redisResult = await _redisConnection.BasicRetryAsync(async db =>
            {
                return await db.StringGetAsync(key);
            });

            if (redisResult.HasValue)
            {
                _logger.LogInformation($"Value found in cache with key: {key}");

#pragma warning disable CS8603 // Possible null reference return.
                return JsonSerializer.Deserialize<WeatherForecast>(redisResult.ToString(), new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
#pragma warning restore CS8603 // Possible null reference return.
            }            
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting cached value");
        }

        return null;
    }

    private async Task CacheValueAsync(string key, WeatherForecast weatherForecast)
    {
        _redisConnection = await _redisConnectionFactory;

        try
        {
            var redisResult = await _redisConnection.BasicRetryAsync(async db =>
            {
                return await db.StringSetAsync(key, JsonSerializer.Serialize(weatherForecast), TimeSpan.FromMinutes(1));
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error caching value");
        }
    }
}

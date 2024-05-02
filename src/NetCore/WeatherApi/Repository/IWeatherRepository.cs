
namespace WeatherApi.Repository
{
    public interface IWeatherRepository
    {
        Task<WeatherForecast> GetWeatherByCityId(int cityId);
        Task<WeatherForecast> GetWeatherByCityName(string cityName);
        Task<IEnumerable<WeatherForecast>> GetWeatherData();
        Task<WeatherForecast> UpdateWeather(int cityId, WeatherForecast updatedWeather);
    }
}
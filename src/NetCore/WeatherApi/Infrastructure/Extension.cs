using Microsoft.ApplicationInsights.Extensibility;
using WeatherApi.Repository;

namespace WeatherApi.Infrastructure;

public static class Extension
{
    public static void RegisterServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IWeatherRepository, WeatherRepository>();

        bool addAppInsighSDK = configuration.GetValue<bool>("UseAppInsightsSDK");

        if (addAppInsighSDK)
        {
            services.AddApplicationInsightsTelemetry();
            services.AddSingleton<ITelemetryInitializer, CloudRoleNameTelemetryInitializer>();
        }

        string redisConnectionString = configuration["Redis"] ?? 
                                        throw new ArgumentNullException("Redis Connection String cannot be null");
        services.AddSingleton(async x => await RedisConnection.InitializeAsync(redisConnectionString));
    }
}

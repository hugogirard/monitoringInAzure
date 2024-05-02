using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.Extensibility;

namespace WeatherApi.Infrastructure
{
    public class CloudRoleNameTelemetryInitializer : ITelemetryInitializer
    {
        private readonly string? _cloudRoleName;

        public CloudRoleNameTelemetryInitializer(IConfiguration configuration)
        {
            _cloudRoleName = configuration["CloudRoleName"] ?? "WeatherApi";
        }

        public void Initialize(ITelemetry telemetry)
        {
            telemetry.Context.Cloud.RoleName = _cloudRoleName;
        }
    }
}

using Microsoft.FeatureManagement;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using StocksDataCollectorAPI.Infrastructure.Telemetry;
using StocksDataCollectorAPI.Services;
using Serilog;

namespace StocksDataCollectorAPI.Extensions
{
  /// <summary>
  /// Provides extension methods for configuring services in an <see cref="IServiceCollection"/>.
  /// </summary>
  public static class ServiceCollectionExtensions
  {
    /// <summary>
    /// Configures services for dependency injection.
    /// </summary>
    public static void ConfigureServices(this IServiceCollection services, IConfiguration configuration)
    {
      // Add Serilog as the primary logger
      services.AddSerilog((services) => services.ReadFrom.Configuration(configuration));

      // API Documentation
      services.AddEndpointsApiExplorer();
      services.AddSwaggerGen();

      // OpenTelemetry
      services.ConfigureOpenTelemetry(configuration);

      // Feature Management
      services.AddFeatureManagement();

      // HTTP Client
      services.AddHttpClient();

      // Application Services
      services.AddSingleton<IMorningstarService, MorningstarService>();

      // Controllers
      services.AddControllers();
    }

    /// <summary>
    /// Configures OpenTelemetry tracing and metrics.
    /// </summary>
    public static void ConfigureOpenTelemetry(this IServiceCollection services, IConfiguration configuration)
    {
      if (OpenTelemetryConfig.TryLoad(configuration, out var config))
      {
        services.AddOpenTelemetry()
            .ConfigureResource(resource => resource.AddService(config.ServiceName))
            .WithTracing(tracing => tracing
                .AddAspNetCoreInstrumentation()
                .AddOtlpExporter(otlpOptions => config.ConfigureExporter(otlpOptions)))
            .WithMetrics(metrics => metrics
                .AddAspNetCoreInstrumentation()
                .AddOtlpExporter(otlpOptions => config.ConfigureExporter(otlpOptions)));
      }
    }
  }
}
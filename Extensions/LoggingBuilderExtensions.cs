using OpenTelemetry.Logs;
using Serilog;
using StocksDataCollectorAPI.Infrastructure.Telemetry;

namespace StocksDataCollectorAPI.Extensions
{
  /// <summary>
  /// Provides extension methods for configuring the logging builder.
  /// </summary>
  public static class LoggingBuilderExtensions
  {
    /// <summary>
    /// Configures logging for the application using OpenTelemetry.
    /// </summary>
    /// <param name="loggingBuilder">The logging builder to configure.</param>
    /// <param name="configuration">The application configuration containing OpenTelemetry settings.</param>
    public static void ConfigureApplicationLogging(this ILoggingBuilder loggingBuilder, IConfiguration configuration)
    {
      // Clear default logging providers
      loggingBuilder.ClearProviders();

      // Add OpenTelemetry for distributed observability
      loggingBuilder.AddOpenTelemetry(options =>
      {
        options.IncludeFormattedMessage = true; // Ensure structured messages
        options.ParseStateValues = true;        // Parse state values for better context

        if (OpenTelemetryConfig.TryLoad(configuration, out var config))
        {
          options.AddOtlpExporter(otlpOptions => config.ConfigureExporter(otlpOptions));
        }
        else
        {
          Log.Warning("OpenTelemetry configuration could not be loaded. Defaulting to local-only logging.");
        }
      });
    }
  }
}
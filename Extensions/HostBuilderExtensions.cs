using Serilog;

namespace StocksDataCollectorAPI.Extensions
{
  /// <summary>
  /// Extension methods for configuring the <see cref="IHostBuilder"/>.
  /// </summary>
  public static class HostBuilderExtensions
  {
    /// <summary>
    /// Configures Serilog and OpenTelemetry logging.
    /// </summary>
    public static void ConfigureLogging(this ConfigureHostBuilder hostBuilder)
    {
      hostBuilder.UseSerilog((context, configuration) =>
      {
        configuration.ReadFrom.Configuration(context.Configuration); // Read from appsettings.json
      });
    }
  }
}
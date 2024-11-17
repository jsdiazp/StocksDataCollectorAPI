using System.Diagnostics.CodeAnalysis;
using OpenTelemetry.Exporter;

namespace StocksDataCollectorAPI.Infrastructure.Telemetry
{
  /// <summary>
  /// Represents OpenTelemetry configuration details.
  /// </summary>
  public class OpenTelemetryConfig
  {
    public required string ServiceName { get; init; }
    public required Uri Endpoint { get; init; }
    public string? Headers { get; init; }

    /// <summary>
    /// Configures the OpenTelemetry exporter options.
    /// </summary>
    public void ConfigureExporter(OtlpExporterOptions options)
    {
      options.Endpoint = Endpoint;
      options.Protocol = OtlpExportProtocol.Grpc;

      if (!string.IsNullOrWhiteSpace(Headers))
      {
        options.Headers = Headers;
      }
    }

    /// <summary>
    /// Attempts to load OpenTelemetry configuration.
    /// </summary>
    public static bool TryLoad(IConfiguration configuration, [NotNullWhen(true)] out OpenTelemetryConfig? config)
    {
      var serviceName = configuration["OPEN_TELEMETRY_SERVICE_NAME"];
      var endpointString = configuration["OPEN_TELEMETRY_EXPLORER_ENDPOINT"];
      var headers = configuration["OPEN_TELEMETRY_EXPLORER_HEADERS"];

      if (!string.IsNullOrWhiteSpace(serviceName) && Uri.TryCreate(endpointString, UriKind.Absolute, out var endpoint))
      {
        config = new OpenTelemetryConfig
        {
          ServiceName = serviceName,
          Endpoint = endpoint,
          Headers = headers
        };
        return true;
      }

      config = null;
      return false;
    }
  }
}
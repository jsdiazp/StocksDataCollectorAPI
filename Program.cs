using OpenTelemetry.Trace;
using OpenTelemetry.Metrics;
using OpenTelemetry.Logs;
using Serilog;
using StocksDataCollectorAPI.Extensions;
using StocksDataCollectorAPI.Extensions.ApplicationExtensions;

// Load environment variables
DotNetEnv.Env.Load();

// Initialize Serilog
Log.Logger = new LoggerConfiguration()
  .WriteTo.Console()
  .CreateBootstrapLogger();

try
{
  var builder = WebApplication.CreateBuilder(args);

  // Configure logging 
  builder.Logging.ConfigureApplicationLogging(builder.Configuration);

  // Configure Services
  builder.Services.ConfigureServices(builder.Configuration);

  // Build the Application
  var app = builder.Build();

  // Configure Middleware and Endpoints
  app.ConfigureMiddleware();
  app.ConfigureEndpoints();

  // Run the Application
  app.Run();
}
catch (Exception ex)
{
  Log.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
  Log.CloseAndFlush();
}
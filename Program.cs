using StocksDataCollectorAPI.Extensions;
using OpenTelemetry.Trace;
using OpenTelemetry.Metrics;
using OpenTelemetry.Logs;

DotNetEnv.Env.Load(); // Load environment variables from .env file

var builder = WebApplication.CreateBuilder(args);

// Configure Host and Services
builder.Host.ConfigureLogging();
builder.Services.ConfigureServices(builder.Configuration);

// Build the Application
var app = builder.Build();

// Configure Middleware and Endpoints
app.ConfigureMiddleware();
app.ConfigureEndpoints();

// Run the Application
app.Run();
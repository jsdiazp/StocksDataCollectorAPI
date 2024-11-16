using StocksDataCollectorAPI.Services;

DotNetEnv.Env.Load(); // Load environment variables from .env file

var builder = WebApplication.CreateBuilder(args);

// Configure Services
ConfigureServices(builder.Services);

var app = builder.Build();

// Configure Middleware and Endpoints
ConfigureMiddleware(app);
ConfigureEndpoints(app);

// Start the Application
app.Run();

/// <summary>
/// Configures the services for the application.
/// </summary>
static void ConfigureServices(IServiceCollection services)
{
  // Add API documentation
  services.AddEndpointsApiExplorer();
  services.AddSwaggerGen();

  // Register application services
  services.AddSingleton<IMorningstarService, MorningstarService>(_ =>
      new MorningstarService(new HttpClient()));

  // Add controllers
  services.AddControllers();
}

/// <summary>
/// Configures middleware for the application.
/// </summary>
static void ConfigureMiddleware(WebApplication app)
{
  if (app.Environment.IsDevelopment())
  {
    app.UseSwagger();
    app.UseSwaggerUI();
  }

  // Add middleware to handle exceptions, authentication, etc., if needed
  app.UseRouting();
  app.UseAuthorization();
}

/// <summary>
/// Configures endpoints for the application.
/// </summary>
static void ConfigureEndpoints(WebApplication app)
{
  app.MapControllers();
}
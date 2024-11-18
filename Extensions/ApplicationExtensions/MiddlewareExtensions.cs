using Serilog;

namespace StocksDataCollectorAPI.Extensions.ApplicationExtensions
{
  /// <summary>
  /// Provides extension methods for configuring middleware for the application.
  /// </summary>
  public static class MiddlewareExtensions
  {
    /// <summary>
    /// Configures middleware for the application.
    /// </summary>
    public static void ConfigureMiddleware(this WebApplication app)
    {
      // Use Serilog request logging
      app.UseSerilogRequestLogging();

      // Configure Swagger
      if (app.Environment.IsDevelopment())
      {
        app.UseSwagger();
        app.UseSwaggerUI();
      }

      // Use default middleware
      app.UseRouting();
      app.UseAuthorization();
    }
  }
}
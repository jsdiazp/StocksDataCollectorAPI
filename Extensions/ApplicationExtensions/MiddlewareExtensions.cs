namespace StocksDataCollectorAPI.Extensions
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
      if (app.Environment.IsDevelopment())
      {
        app.UseSwagger();
        app.UseSwaggerUI();
      }

      app.UseRouting();
      app.UseAuthorization();
    }
  }
}
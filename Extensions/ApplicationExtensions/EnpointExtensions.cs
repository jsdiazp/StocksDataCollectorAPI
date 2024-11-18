namespace StocksDataCollectorAPI.Extensions.ApplicationExtensions
{
  public static class EndpointExtensions
  {
    /// <summary>
    /// Configures endpoints for the application.
    /// </summary>
    public static void ConfigureEndpoints(this WebApplication app)
    {
      app.MapControllers();
    }
  }
}
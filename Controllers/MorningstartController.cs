using Microsoft.AspNetCore.Mvc;
using Microsoft.FeatureManagement.Mvc;
using StocksDataCollectorAPI.Services;

namespace StocksDataCollectorAPI.Controllers
{
  [ApiController]
  [Route("api/[controller]")]
  public class MorningstarController(IMorningstarService morningstarService) : ControllerBase
  {
    private readonly IMorningstarService _morningstarService = morningstarService ?? throw new ArgumentNullException(nameof(morningstarService));

    /// <summary>
    /// Extract specific stock data fields for a given performance ID.
    /// </summary>
    /// <param name="performanceId">The performance ID.</param>
    /// <param name="fields">The fields to extract.</param>
    /// <returns>A dictionary containing the requested fields and their values.</returns>
    [HttpPost("{performanceId}/Extract")]
    public async Task<IActionResult> ExtractStockDataAsync(string performanceId, [FromBody] string[] fields)
    {
      if (string.IsNullOrWhiteSpace(performanceId))
        return BadRequest("Stock ID cannot be null or empty.");

      if (fields == null || fields.Length == 0)
        return BadRequest("Fields array cannot be null or empty.");

      var result = await _morningstarService.ExtractStockDataAsync(performanceId, fields);
      return result != null ? Ok(result) : NotFound($"Data not found for stock ID: {performanceId}");
    }

    /// <summary>
    /// Get valuation data for a specific performance ID.
    /// </summary>
    /// <param name="performanceId">The performance ID.</param>
    /// <returns>The valuation data.</returns>
    [HttpGet("{performanceId}/Valuation")]
    public async Task<IActionResult> GetValuationDataAsync(string performanceId)
    {
      if (string.IsNullOrWhiteSpace(performanceId))
        return BadRequest("Stock ID cannot be null or empty.");

      var result = await _morningstarService.GetValuationDataAsync(performanceId);
      return result != null ? Ok(result) : NotFound($"Valuation data not found for stock ID: {performanceId}");
    }

    /// <summary>
    /// Get operating performance data for a specific performance ID.
    /// </summary>
    /// <param name="performanceId">The performance ID.</param>
    /// <returns>The operating performance data.</returns>
    [HttpGet("{performanceId}/OperatingPerformance")]
    public async Task<IActionResult> GetOperatingPerformanceDataAsync(string performanceId)
    {
      if (string.IsNullOrWhiteSpace(performanceId))
        return BadRequest("Stock ID cannot be null or empty.");

      var result = await _morningstarService.GetOperatingPerformanceDataAsync(performanceId);
      return result != null ? Ok(result) : NotFound($"Operating performance data not found for performance ID: {performanceId}");
    }

    /// <summary>
    /// Get the stock performance ID for a given stock exchange and stock ticker.
    /// </summary>
    /// <param name="stockExchange">The stock exchange.</param>
    /// <param name="stockTicker">The stock ticker.</param>
    /// <returns>The stock performance ID.</returns>
    [HttpGet("{stockExchange}:{stockTicker}/PerformanceId")]
    [FeatureGate("MorningstarService_GetStockPerformanceId")]
    public async Task<IActionResult> GetStockPerformanceIDAsync(string stockExchange, string stockTicker)
    {
      var result = await _morningstarService.GetStockPerformanceIDAsync(stockExchange, stockTicker);
      return result != null ? Ok(result) : NotFound($"Stock performance ID not found for stock exchange: {stockExchange}, stock ticker: {stockTicker}");
    }

    /// <summary>
    /// Get stock data for a specific performance ID.
    /// </summary>
    /// <param name="performanceId">The performance ID.</param>
    /// <returns>The stock data.</returns>
    [HttpGet("{performanceId}")]
    public async Task<IActionResult> GetStockDataAsync(string performanceId)
    {
      if (string.IsNullOrWhiteSpace(performanceId))
        return BadRequest("Stock ID cannot be null or empty.");

      var result = await _morningstarService.GetStockDataAsync(performanceId);
      return result != null ? Ok(result) : NotFound($"Stock data not found for stock ID: {performanceId}");
    }
  }
}
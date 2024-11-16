using Microsoft.AspNetCore.Mvc;
using StocksDataCollectorAPI.Services;

namespace StocksDataCollectorAPI.Controllers
{
  [ApiController]
  [Route("api/[controller]")]
  public class MorningstarController(IMorningstarService morningstarService) : ControllerBase
  {
    private readonly IMorningstarService _morningstarService = morningstarService ?? throw new ArgumentNullException(nameof(morningstarService));

    /// <summary>
    /// Extract specific stock data fields for a given stock ID.
    /// </summary>
    /// <param name="id">The stock ID.</param>
    /// <param name="fields">The fields to extract.</param>
    /// <returns>A dictionary containing the requested fields and their values.</returns>
    [HttpPost("{id}/Extract")]
    public async Task<IActionResult> ExtractStockDataAsync(string id, [FromBody] string[] fields)
    {
      if (string.IsNullOrWhiteSpace(id))
        return BadRequest("Stock ID cannot be null or empty.");

      if (fields == null || fields.Length == 0)
        return BadRequest("Fields array cannot be null or empty.");

      var result = await _morningstarService.ExtractStockDataAsync(id, fields);
      return result != null ? Ok(result) : NotFound($"Data not found for stock ID: {id}");
    }

    /// <summary>
    /// Get valuation data for a specific stock ID.
    /// </summary>
    /// <param name="id">The stock ID.</param>
    /// <returns>The valuation data.</returns>
    [HttpGet("{id}/Valuation")]
    public async Task<IActionResult> GetValuationDataAsync(string id)
    {
      if (string.IsNullOrWhiteSpace(id))
        return BadRequest("Stock ID cannot be null or empty.");

      var result = await _morningstarService.GetValuationDataAsync(id);
      return result != null ? Ok(result) : NotFound($"Valuation data not found for stock ID: {id}");
    }

    /// <summary>
    /// Get operating performance data for a specific stock ID.
    /// </summary>
    /// <param name="id">The stock ID.</param>
    /// <returns>The operating performance data.</returns>
    [HttpGet("{id}/OperatingPerformance")]
    public async Task<IActionResult> GetOperatingPerformanceDataAsync(string id)
    {
      if (string.IsNullOrWhiteSpace(id))
        return BadRequest("Stock ID cannot be null or empty.");

      var result = await _morningstarService.GetOperatingPerformanceDataAsync(id);
      return result != null ? Ok(result) : NotFound($"Operating performance data not found for stock ID: {id}");
    }

    /// <summary>
    /// Get stock data for a specific stock ID.
    /// </summary>
    /// <param name="id">The stock ID.</param>
    /// <returns>The stock data.</returns>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetStockDataAsync(string id)
    {
      if (string.IsNullOrWhiteSpace(id))
        return BadRequest("Stock ID cannot be null or empty.");

      var result = await _morningstarService.GetStockDataAsync(id);
      return result != null ? Ok(result) : NotFound($"Stock data not found for stock ID: {id}");
    }
  }
}
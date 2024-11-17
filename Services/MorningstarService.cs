using Microsoft.FeatureManagement.Mvc;
using StocksDataCollectorAPI.Helpers;
using StocksDataCollectorAPI.Models.Morningstar;

namespace StocksDataCollectorAPI.Services
{
  public class MorningstarService : IMorningstarService
  {
    private readonly ILogger<MorningstarService> _logger;
    private readonly HttpClient _httpClient;
    private const string BaseUrl = "https://api-global.morningstar.com/sal-service/v1/stock";

    public MorningstarService(ILogger<MorningstarService> logger, HttpClient httpClient)
    {
      _logger = logger;
      _httpClient = httpClient;
      _httpClient.DefaultRequestHeaders.Add("ApiKey", Environment.GetEnvironmentVariable("MORNINGSTAR_API_KEY"));
      _httpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/89.0.142.86 Safari/537.36");
    }

    public async Task<string?> GetStockPerformanceIDAsync(string stockExchange, string stockTicker)
    {
      var baseUrl = "https://www.morningstar.com/api/v2";
      var endpoint = $"stores/realtime/quotes?securities={stockExchange}:{stockTicker}";

      var result = await GetDataAsync<Dictionary<string, RealTimeQuoteData>>(endpoint, baseUrl);

      if (result == null)
      {
        _logger.LogInformation("Unable to get stock performance ID for {stockExchange}:{stockTicker}", stockExchange, stockTicker);
        return null;
      }

      return DotNotationHelper.GetNestedProperty(result, $"{stockExchange}:{stockTicker}.performanceId.value");
    }

    public async Task<StockData?> GetStockDataAsync(string stockID)
    {
      var operatingPerformanceTask = GetOperatingPerformanceDataAsync(stockID);
      var valuationTask = GetValuationDataAsync(stockID);
      var trailingTotalReturnsTask = GetTrailingTotalReturnsDataAsync(stockID);

      await Task.WhenAll(operatingPerformanceTask, valuationTask, trailingTotalReturnsTask);

      if (operatingPerformanceTask.Result == null && valuationTask.Result == null && trailingTotalReturnsTask.Result == null)
      {
        return null;
      }

      return new StockData
      {
        operatingPerformanceData = operatingPerformanceTask.Result,
        valuationData = valuationTask.Result,
        trailingTotalReturnsListData = trailingTotalReturnsTask.Result
      };
    }

    public async Task<Dictionary<string, object?>?> ExtractStockDataAsync(string stockID, string[] fields)
    {
      var stockData = await GetStockDataAsync(stockID);
      var value = new Dictionary<string, object?>(fields.Length);

      if (stockData == null)
      {
        return null;
      }

      foreach (var field in fields)
      {
        value[field] = DotNotationHelper.GetNestedProperty(stockData, field);
      }

      return value;
    }

    public async Task<ValuationData?> GetValuationDataAsync(string stockID)
    {
      return await GetDataAsync<ValuationData>($"valuation/v3/{stockID}");
    }

    public async Task<OperatingPerformanceData?> GetOperatingPerformanceDataAsync(string stockID)
    {
      return await GetDataAsync<OperatingPerformanceData>($"operatingPerformance/v3/{stockID}");
    }

    public async Task<TrailingTotalReturnsListData?> GetTrailingTotalReturnsDataAsync(string stockID)
    {
      return await GetDataAsync<TrailingTotalReturnsListData>($"trailingTotalReturns/{stockID}/data");
    }

    private async Task<T?> GetDataAsync<T>(string endpoint, string baseUrl = BaseUrl)
    {
      try
      {
        var response = await _httpClient.GetAsync($"{baseUrl}/{endpoint}");

        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<T>();
      }
      catch (HttpRequestException httpEx)
      {
        _logger.LogError(httpEx, "HTTP request error.");
        return default;
      }
      catch (Exception ex)
      {
        _logger.LogError(ex, "Unexpected error.");
        return default;
      }
    }
  }
}
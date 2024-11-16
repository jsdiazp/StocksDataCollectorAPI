using StocksDataCollectorAPI.Helpers;
using StocksDataCollectorAPI.Models.Morningstar;

namespace StocksDataCollectorAPI.Services
{
  public class MorningstarService : IMorningstarService
  {
    private readonly HttpClient _httpClient;
    private const string BaseUrl = "https://api-global.morningstar.com/sal-service/v1/stock";

    public MorningstarService(HttpClient httpClient)
    {
      _httpClient = httpClient;
      _httpClient.DefaultRequestHeaders.Add("ApiKey", Environment.GetEnvironmentVariable("API_KEY"));
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

    private async Task<T?> GetDataAsync<T>(string endpoint)
    {
      try
      {
        var response = await _httpClient.GetAsync($"{BaseUrl}/{endpoint}?languageId=en&locale=en&clientId=MDC&component=sal-valuation&version=4.30.0");

        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<T>();
      }
      catch (HttpRequestException httpEx)
      {
        Console.WriteLine($"Request error: {httpEx.Message}");
        return default;
      }
      catch (Exception ex)
      {
        Console.WriteLine($"Unexpected error: {ex.Message}");
        return default;
      }
    }
  }
}
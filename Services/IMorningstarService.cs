using StocksDataCollectorAPI.Models.Morningstar;

namespace StocksDataCollectorAPI.Services
{
  public interface IMorningstarService
  {
    Task<string?> GetStockPerformanceIDAsync(string stockExchange, string stockTicker);
    Task<StockData?> GetStockDataAsync(string stockID);
    Task<ValuationData?> GetValuationDataAsync(string stockID);
    Task<TrailingTotalReturnsListData?> GetTrailingTotalReturnsDataAsync(string stockID);
    Task<OperatingPerformanceData?> GetOperatingPerformanceDataAsync(string stockID);
    Task<Dictionary<string, object?>?> ExtractStockDataAsync(string stockID, string[] fields);

  }
}
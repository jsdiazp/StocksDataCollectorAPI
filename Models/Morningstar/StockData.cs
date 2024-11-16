
namespace StocksDataCollectorAPI.Models.Morningstar
{
  public class StockData
  {
    public OperatingPerformanceData? operatingPerformanceData { get; set; }
    public ValuationData? valuationData { get; set; }
    public TrailingTotalReturnsListData? trailingTotalReturnsListData { get; set; }
  }
}
namespace StocksDataCollectorAPI.Models
{
  public class Stock
  {
    public string Ticker { get; set; } = string.Empty;
    public string StockExchange { get; set; } = string.Empty;
    public string MorningstarID { get; set; } = string.Empty;
    public string CompanyName { get; set; } = string.Empty;
  }
}
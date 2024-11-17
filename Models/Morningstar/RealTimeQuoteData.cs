using System.Security.Cryptography.X509Certificates;

namespace StocksDataCollectorAPI.Models.Morningstar
{
  public class RealtimeQuoteDataItem
  {
    public string? value { get; set; }
  };
  public class RealTimeQuoteData
  {

    public RealtimeQuoteDataItem? performanceId { get; set; }
    public RealtimeQuoteDataItem? name { get; set; }
    public RealtimeQuoteDataItem? exchange { get; set; }
  };
}
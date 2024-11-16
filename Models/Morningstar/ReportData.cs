namespace StocksDataCollectorAPI.Models.Morningstar
{
  public class ReportData
  {
    public string? reportType { get; set; }
    public string? reportType_label { get; set; }
    public List<string>? columnDefs { get; set; }
    public List<string>? columnDefs_labels { get; set; }
    public TableData? Collapsed { get; set; }
    public TableData? Expanded { get; set; }
  }
}
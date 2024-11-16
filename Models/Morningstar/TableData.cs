namespace StocksDataCollectorAPI.Models.Morningstar
{
  public class TableData
  {
    public List<RowData>? rows { get; set; }
    public List<string>? columnDefs { get; set; }
    public List<string>? columnDefs_labels { get; set; }
  }
}
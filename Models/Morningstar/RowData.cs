namespace StocksDataCollectorAPI.Models.Morningstar
{

  public class RowData
  {
    public string? label { get; set; }
    public string? salDataId { get; set; }
    public List<float?>? datum { get; set; }
    public string? subLevel { get; set; }
    public bool? percentage { get; set; }
  }
}
namespace StocksDataCollectorAPI.Models.Morningstar
{
  public class TrailingTotalReturnData
  {
    public string? name { get; set; }
    public double? trailing1DayReturn
    { get; set; }
    public double? trailing1WeekReturn { get; set; }
    public double? trailing1MonthReturn { get; set; }
    public double? trailing3MonthReturn { get; set; }
    public double? trailing6MonthReturn { get; set; }
    public double? trailingYearToDateReturn { get; set; }
    public double? trailing1YearReturn { get; set; }
    public double? trailing3YearReturn { get; set; }
    public double? trailing5YearReturn { get; set; }
    public double? trailing10YearReturn { get; set; }
    public double? trailing15YearReturn { get; set; }
  }
}
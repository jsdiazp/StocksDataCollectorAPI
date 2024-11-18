using Microsoft.AspNetCore.Mvc;
using Moq;
using StocksDataCollectorAPI.Models.Morningstar;
using StocksDataCollectorAPI.Services;
using Xunit;

namespace StocksDataCollectorAPI.Controllers.Tests
{
  public class MorningstarControllerTests
  {
    private readonly Mock<IMorningstarService> _mockMorningstarService;
    private readonly MorningstarController _controller;

    private const string ValidPerformanceID = "0P000000GY";
    private const string ValidStockExchange = "XNAS";
    private const string ValidStockTicker = "AAPL";
    private const string InvalidPerformanceID = "invalid_performance_id";
    private const string InvalidStockExchange = "invalid_stock_exchange";
    private const string InvalidStockTicker = "invalid_stock_ticker";

    public MorningstarControllerTests()
    {
      _mockMorningstarService = new Mock<IMorningstarService>();
      ConfigureMockHttpResponses();
      _controller = new MorningstarController(_mockMorningstarService.Object);
    }

    [Theory]
    [MemberData(nameof(ValuationDataTestCases))]
    public async Task GetValuationDataAsync_ReturnsExpectedResults(string performanceId, Type? expectedResultType)
    {
      // Arrange & Act
      var result = await _controller.GetValuationDataAsync(performanceId);

      // Assert
      AssertResponseType(result, expectedResultType);
    }

    [Theory]
    [MemberData(nameof(OperatingPerformanceDataTestCases))]
    public async Task GetOperatingPerformanceDataAsync_ReturnsExpectedResults(string performanceId, Type? expectedResultType)
    {
      // Arrange & Act
      var result = await _controller.GetOperatingPerformanceDataAsync(performanceId);

      // Assert
      AssertResponseType(result, expectedResultType);
    }

    [Theory]
    [MemberData(nameof(StockDataTestCases))]
    public async Task GetStockDataAsync_ReturnsExpectedResults(string performanceId, Type? expectedResultType)
    {
      // Arrange & Act
      var result = await _controller.GetStockDataAsync(performanceId);

      // Assert
      AssertResponseType(result, expectedResultType);
    }

    [Theory]
    [MemberData(nameof(PerformanceDataTestCases))]
    public async Task GetPerformanceDataAsync_ReturnsExpectedResults(string stockExchange, string stockTicker, string? expectedValue)
    {
      // Arrange & Act
      var result = await _controller.GetStockPerformanceIDAsync(stockExchange, stockTicker);

      // Assert
      if (expectedValue == null)
      {
        Assert.IsType<NotFoundObjectResult>(result);
      }
      else
      {
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(expectedValue, okResult.Value);
      }
    }

    #region Test Helpers and Data Providers

    private void AssertResponseType(IActionResult result, Type? expectedType)
    {
      if (expectedType == null)
      {
        Assert.IsType<NotFoundObjectResult>(result);
      }
      else
      {
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.IsType(expectedType, okResult.Value);
      }
    }

    private void ConfigureMockHttpResponses()
    {
      _mockMorningstarService
        .Setup(s => s.GetValuationDataAsync(ValidPerformanceID))
        .ReturnsAsync(new ValuationData());
      _mockMorningstarService
        .Setup(s => s.GetOperatingPerformanceDataAsync(ValidPerformanceID))
        .ReturnsAsync(new OperatingPerformanceData());
      _mockMorningstarService
        .Setup(s => s.GetStockDataAsync(ValidPerformanceID))
        .ReturnsAsync(new StockData());
      _mockMorningstarService
        .Setup(s => s.GetStockPerformanceIDAsync(ValidStockExchange, ValidStockTicker))
        .ReturnsAsync(ValidPerformanceID);
    }

    public static IEnumerable<object?[]> ValuationDataTestCases =>
      [
        [ InvalidPerformanceID, null ],
        [ ValidPerformanceID, typeof(ValuationData) ]
      ];

    public static IEnumerable<object?[]> OperatingPerformanceDataTestCases =>
      [
        [ InvalidPerformanceID, null ],
        [ ValidPerformanceID, typeof(OperatingPerformanceData) ]
      ];

    public static IEnumerable<object?[]> StockDataTestCases =>
      [
        [ InvalidPerformanceID, null ],
        [ ValidPerformanceID, typeof(StockData) ]
      ];

    public static IEnumerable<object?[]> PerformanceDataTestCases =>
      [
        [ InvalidStockExchange, InvalidStockTicker, null ],
        [ ValidStockExchange, InvalidStockTicker, null ],
        [ InvalidStockExchange, ValidStockTicker, null ],
        [ ValidStockExchange, ValidStockTicker, ValidPerformanceID ]
      ];

    #endregion
  }
}
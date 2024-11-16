using System.Net;
using System.Text.Json;
using Moq;
using Moq.Protected;
using StocksDataCollectorAPI.Models.Morningstar;
using Xunit;

namespace StocksDataCollectorAPI.Services.Tests
{
  public class MorningstarServiceTests
  {
    private readonly Mock<HttpMessageHandler> _mockHttpHandler;
    private readonly HttpClient _httpClient;
    private readonly MorningstarService _morningstarService;

    private const string ValidStockID = "0P000003RE";

    public MorningstarServiceTests()
    {
      _mockHttpHandler = new Mock<HttpMessageHandler>();
      _httpClient = new HttpClient(_mockHttpHandler.Object);
      _morningstarService = new MorningstarService(_httpClient);

      ConfigureMockHttpResponses();
    }

    [Theory]
    [InlineData("invalid_stock_id", null)]
    [InlineData(ValidStockID, typeof(ValuationData))]
    public async Task GetValuationDataAsync_ValidatesResponses(string stockId, Type? expectedType)
    {
      // Act
      var result = await _morningstarService.GetValuationDataAsync(stockId);

      // Assert
      if (expectedType == null)
        Assert.Null(result);
      else
        Assert.IsType(expectedType, result);
    }

    [Theory]
    [InlineData("invalid_stock_id", null)]
    [InlineData(ValidStockID, typeof(OperatingPerformanceData))]
    public async Task GetOperatingPerformanceDataAsync_ValidatesResponses(string stockId, Type? expectedType)
    {
      // Act
      var result = await _morningstarService.GetOperatingPerformanceDataAsync(stockId);

      // Assert
      if (expectedType == null)
        Assert.Null(result);
      else
        Assert.IsType(expectedType, result);
    }

    [Theory]
    [InlineData("invalid_stock_id", null)]
    [InlineData(ValidStockID, typeof(TrailingTotalReturnsListData))]
    public async Task GetTrailingTotalReturnsDataAsync_ValidatesResponses(string stockId, Type? expectedType)
    {
      // Act
      var result = await _morningstarService.GetTrailingTotalReturnsDataAsync(stockId);

      // Assert
      if (expectedType == null)
        Assert.Null(result);
      else
        Assert.IsType(expectedType, result);
    }

    [Theory]
    [InlineData("invalid_stock_id", null)]
    [InlineData(ValidStockID, typeof(StockData))]
    public async Task GetStockDataAsync_ValidatesResponses(string stockId, Type? expectedType)
    {
      // Act
      var result = await _morningstarService.GetStockDataAsync(stockId);

      // Assert
      if (expectedType == null)
        Assert.Null(result);
      else
        Assert.IsType(expectedType, result);
    }

    private void ConfigureMockHttpResponses()
    {
      _mockHttpHandler
          .Protected()
          .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
          .ReturnsAsync((HttpRequestMessage request, CancellationToken cancellationToken) =>
          {
            if (request.RequestUri == null) return new HttpResponseMessage(HttpStatusCode.NotFound);

            return request.RequestUri.ToString() switch
            {
              string uri when uri.Contains($"valuation/v3/{ValidStockID}") =>
                        CreateHttpResponse(new ValuationData()),
              string uri when uri.Contains($"operatingPerformance/v3/{ValidStockID}") =>
                        CreateHttpResponse(new OperatingPerformanceData()),
              string uri when uri.Contains($"trailingTotalReturns/{ValidStockID}/data") =>
                        CreateHttpResponse(new TrailingTotalReturnsListData()),
              _ => new HttpResponseMessage(HttpStatusCode.NotFound)
            };
          });
    }

    private static HttpResponseMessage CreateHttpResponse<T>(T data)
    {
      return new HttpResponseMessage(HttpStatusCode.OK)
      {
        Content = new StringContent(JsonSerializer.Serialize(data))
      };
    }

    private static StockData CreateValidStockData()
    {
      return new StockData
      {
        operatingPerformanceData = new OperatingPerformanceData(),
        valuationData = new ValuationData(),
        trailingTotalReturnsListData = new TrailingTotalReturnsListData()
      };
    }
  }
}
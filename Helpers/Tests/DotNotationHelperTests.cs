using Xunit;

namespace StocksDataCollectorAPI.Helpers.Tests
{
  public class DotNotationHelperTests
  {
    [Theory]
    [InlineData(null, "property", null)] // Null source
    [InlineData(default, "", null)] // Empty path
    [InlineData(default, "invalidProperty", null)] // Invalid property
    public void GetNestedProperty_InvalidInputs_ReturnsNull(object? source, string path, object? expected)
    {
      // Act
      var result = DotNotationHelper.GetNestedProperty(source, path);

      // Assert
      Assert.Equal(expected, result);
    }


    [Fact]
    public void GetNestedProperty_NestedPropertyExists_ReturnsValue()
    {
      // Arrange
      var source = new { Foo = new { Bar = "baz" } };
      const string path = "Foo.Bar";

      // Act
      var result = DotNotationHelper.GetNestedProperty(source, path);

      // Assert
      Assert.Equal("baz", result);
    }

    [Fact]
    public void GetNestedProperty_NestedPropertyDoesNotExist_ReturnsNull()
    {
      // Arrange
      var source = new { Foo = new { } };
      const string path = "Foo.Bar";

      // Act
      var result = DotNotationHelper.GetNestedProperty(source, path);

      // Assert
      Assert.Null(result);
    }

    [Fact]
    public void GetNestedProperty_PropertyWithNullValue_ReturnsNull()
    {
      // Arrange
      var source = new { Foo = default(object) };
      const string path = "Foo";

      // Act
      var result = DotNotationHelper.GetNestedProperty(source, path);

      // Assert
      Assert.Null(result);
    }
  }
}
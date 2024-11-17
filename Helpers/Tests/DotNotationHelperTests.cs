using Xunit;

namespace StocksDataCollectorAPI.Helpers.Tests
{
  public class DotNotationHelperTests
  {
    /// <summary>
    /// Verifies that invalid inputs return null.
    /// </summary>
    [Theory]
    [InlineData(null, "property", null)] // Null source
    [InlineData(null, "", null)] // Empty path
    [InlineData(null, "invalidProperty", null)] // Invalid property
    public void GetNestedProperty_InvalidInputs_ReturnsNull(object? source, string path, object? expected)
    {
      // Act
      var result = DotNotationHelper.GetNestedProperty(source, path);

      // Assert
      Assert.Equal(expected, result);
    }

    /// <summary>
    /// Provides valid test data for nested properties, including collections and dictionaries.
    /// </summary>
    public static IEnumerable<object[]> ValidNestedPropertyTestData =>
      [
        [ new { Foo = new { Bar = 1 } }, "Foo.Bar", 1 ],
        [ new { Foo = new { Bar = new[] { 1, 2, 3 } } }, "Foo.Bar[1]", 2 ],
        [ new Dictionary<string, object?> { { "Foo", new { Bar = 1 } } }, "Foo.Bar", 1 ]
      ];

    /// <summary>
    /// Verifies that existing nested properties are resolved correctly.
    /// </summary>
    [Theory]
    [MemberData(nameof(ValidNestedPropertyTestData))]
    public void GetNestedProperty_ValidNestedProperty_ReturnsValue(object source, string path, object expected)
    {
      // Act
      var result = DotNotationHelper.GetNestedProperty(source, path);

      // Assert
      Assert.Equal(expected, result);
    }

    /// <summary>
    /// Verifies that a nonexistent nested property returns null.
    /// </summary>
    [Fact]
    public void GetNestedProperty_NonexistentNestedProperty_ReturnsNull()
    {
      // Arrange
      var source = new { Foo = new { } };
      const string path = "Foo.Bar";

      // Act
      var result = DotNotationHelper.GetNestedProperty(source, path);

      // Assert
      Assert.Null(result);
    }

    /// <summary>
    /// Verifies that a property with a null value returns null.
    /// </summary>
    [Fact]
    public void GetNestedProperty_NullValueProperty_ReturnsNull()
    {
      // Arrange
      var source = new { Foo = (object?)null };
      const string path = "Foo";

      // Act
      var result = DotNotationHelper.GetNestedProperty(source, path);

      // Assert
      Assert.Null(result);
    }
  }
}
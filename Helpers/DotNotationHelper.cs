using System.Collections;
using System.Reflection;
using System.Text.RegularExpressions;

namespace StocksDataCollectorAPI.Helpers
{
  /// <summary>
  /// A utility class for retrieving values from objects using dot notation paths, supporting array indexing.
  /// </summary>
  public static partial class DotNotationHelper
  {
    [GeneratedRegex(@"^([a-zA-Z_][a-zA-Z0-9_]*)\[(\d+)\]$")]
    private static partial Regex ArrayIndexRegex();

    /// <summary>
    /// Retrieves the value of a nested property from an object using a dot notation path.
    /// </summary>
    /// <param name="source">The source object from which to retrieve the value.</param>
    /// <param name="path">The dot notation path to the nested property. Supports array indexing.</param>
    /// <returns>The value of the nested property if it exists; otherwise, null.</returns>
    public static dynamic? GetNestedProperty(object? source, string? path)
    {
      // Return null for invalid input.
      if (source == null || string.IsNullOrWhiteSpace(path))
        return null;

      var properties = path.Split('.');
      dynamic? current = source;

      foreach (var property in properties)
      {
        if (current == null)
          return null;

        current = GetPropertyValue(current, property);
        if (current == null)
          return null;
      }

      return current;
    }

    /// <summary>
    /// Resolves a property value, supporting both regular properties and array indexing.
    /// </summary>
    /// <param name="source">The object containing the property.</param>
    /// <param name="propertyPath">The property name or array index path.</param>
    /// <returns>The resolved value if the property exists; otherwise, null.</returns>
    private static dynamic? GetPropertyValue(dynamic source, string propertyPath)
    {
      var type = source.GetType();
      var match = ArrayIndexRegex().Match(propertyPath);

      if (match.Success)
      {
        // Handle array indexing.
        return GetArrayElement(source, type, match.Groups[1].Value, match.Groups[2].Value);
      }
      else
      {
        // Handle regular property.
        var propertyInfo = type.GetProperty(propertyPath, BindingFlags.Instance | BindingFlags.Public);
        return propertyInfo?.GetValue(source);
      }
    }

    /// <summary>
    /// Resolves a value from a list using array indexing.
    /// </summary>
    /// <param name="source">The object containing the list property.</param>
    /// <param name="type">The type of the source object.</param>
    /// <param name="propertyName">The name of the list property.</param>
    /// <param name="indexString">The index string from the path.</param>
    /// <returns>The element at the specified index if it exists; otherwise, null.</returns>
    private static dynamic? GetArrayElement(dynamic source, Type type, string propertyName, string indexString)
    {
      // Validate index.
      if (!int.TryParse(indexString, out var index) || index < 0)
        return null;

      // Retrieve the list property.
      var propertyInfo = type.GetProperty(propertyName, BindingFlags.Instance | BindingFlags.Public);
      var list = propertyInfo?.GetValue(source) as IList;

      // Check if it's a valid list and if the index is within range.
      return list != null && index < list.Count ? list[index] : null;
    }
  }
}
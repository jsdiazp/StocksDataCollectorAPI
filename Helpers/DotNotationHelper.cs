using System.Collections;
using System.Reflection;
using System.Text.RegularExpressions;

namespace StocksDataCollectorAPI.Helpers
{
  /// <summary>
  /// Provides utilities for accessing nested properties and collections using dot notation with optional array indexing.
  /// </summary>
  public static partial class DotNotationHelper
  {
    [GeneratedRegex(@"^([a-zA-Z_][a-zA-Z0-9_]*)\[(\d+)\]$", RegexOptions.Compiled)]
    private static partial Regex ArrayIndexRegex();

    /// <summary>
    /// Retrieves the value of a nested property from an object or collection using a dot notation path, supporting array indexing.
    /// </summary>
    /// <param name="source">The object or collection to traverse.</param>
    /// <param name="path">The dot notation path to the nested property.</param>
    /// <returns>The resolved value or <c>null</c> if the path cannot be resolved.</returns>
    public static dynamic? GetNestedProperty(dynamic? source, string? path)
    {
      if (source is null || string.IsNullOrWhiteSpace(path))
        return null;

      var properties = path.Split('.');
      dynamic? current = source;

      foreach (var property in properties)
      {
        if (current is null) return null;

        current = ResolvePropertyValue(current, property);
      }

      return current;
    }

    /// <summary>
    /// Resolves a property value from an object or collection.
    /// </summary>
    /// <param name="source">The object containing the property or collection.</param>
    /// <param name="propertyPath">The name of the property or the collection index accessor.</param>
    /// <returns>The resolved value or <c>null</c> if not found.</returns>
    private static object? ResolvePropertyValue(object source, string propertyPath)
    {
      if (source is IDictionary dictionary && dictionary.Contains(propertyPath))
      {
        return dictionary[propertyPath];
      }

      var match = ArrayIndexRegex().Match(propertyPath);
      if (match.Success)
      {
        var propertyName = match.Groups[1].Value;
        var index = int.Parse(match.Groups[2].Value);

        return ResolveIndexedPropertyValue(source, propertyName, index);
      }

      var propertyInfo = source.GetType().GetProperty(propertyPath, BindingFlags.Instance | BindingFlags.Public);
      return propertyInfo?.GetValue(source);
    }

    /// <summary>
    /// Retrieves an indexed value from a collection property.
    /// </summary>
    /// <param name="source">The object containing the collection property.</param>
    /// <param name="propertyName">The name of the collection property.</param>
    /// <param name="index">The index of the element to retrieve.</param>
    /// <returns>The indexed element or <c>null</c> if not found.</returns>
    private static object? ResolveIndexedPropertyValue(object source, string propertyName, int index)
    {
      if (index < 0) return null;

      var propertyInfo = source.GetType().GetProperty(propertyName, BindingFlags.Instance | BindingFlags.Public);
      if (propertyInfo is null) return null;

      if (propertyInfo.GetValue(source) is IList list && index < list.Count)
      {
        return list[index];
      }

      return null;
    }
  }
}
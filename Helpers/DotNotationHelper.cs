using System.Collections;
using System.Text.RegularExpressions;

namespace StocksDataCollectorAPI.Helpers
{
  public static partial class DotNotationHelper
  {
    [GeneratedRegex(@"^([a-zA-Z_][a-zA-Z0-9_]*)\[(\d+)\]$")]
    private static partial Regex Regex();
    public static dynamic? GetNestedProperty(object? source, string path)
    {
      if (source == null || string.IsNullOrWhiteSpace(path))
      {
        return null;
      }

      var properties = path.Split('.');

      dynamic? current = source;

      foreach (var property in properties)
      {
        if (current == null)
        {
          return null;
        }

        var type = current.GetType();
        var match = Regex().Match(property);
        if (match.Success)
        {
          // Handle array index
          var propertyName = match.Groups[1].Value;
          if (!int.TryParse(match.Groups[2].Value, out var index))
          {
            return null; // Invalid index format
          }

          var propertyInfo = type.GetProperty(propertyName);
          if (propertyInfo == null)
          {
            return null;
          }

          current = propertyInfo.GetValue(current);
          if (current is not IList list || index < 0 || index >= list.Count)
          {
            return null; // Not a list or index out of range
          }

          current = list[index];
        }
        else
        {
          // Handle regular property
          var propertyInfo = type.GetProperty(property);
          if (propertyInfo == null)
          {
            return null;
          }

          current = propertyInfo.GetValue(current);
        }
      }

      return current;
    }
  }
}
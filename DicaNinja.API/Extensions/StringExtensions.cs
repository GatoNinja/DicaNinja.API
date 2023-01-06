using System.Globalization;

namespace DicaNinja.API.Extensions;

public static class StringExtensions
{
    public static string Clean(this string value)
    {
        return value.Trim().ToLowerInvariant();
    }
}

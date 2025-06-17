using System.Text.RegularExpressions;

namespace Application.Helpers;

public static class TagHelpers
{
    public static List<string> ExtractLabels(string description)
    {
        return Regex.Matches(description, @"#(\w+)")
            .Select(m => m.Groups[1].Value.ToLowerInvariant())
            .Distinct()
            .ToList();
    }
}
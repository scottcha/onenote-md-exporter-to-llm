using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;

public class LinkExtractor
{
    public static List<string> ExtractHttpLinks(string input)
    {
        var httpLinks = new List<string>();
        string pattern = @"(?:<|\()?((http|https):\/\/[^\s<>\\)]+)(?:>|\))?";
        var matches = Regex.Matches(input, pattern);

        foreach (Match match in matches)
        {
            if (httpLinks.Count < 2)
            {
                httpLinks.Add(match.Groups[1].Value);
            }
            else
            {
                break;
            }
        }

        return httpLinks;
    }
}
using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;

public class LinkExtractor
{
    public static string ExtractHttpLinks(string input)
    {
        var httpLinks = new List<string>();

        string pattern = @"(?:<|\()?((http|https):\/\/[^\s<>\\)]+)(?:>|\))?";


        var match = Regex.Match(input, pattern);

        if (match.Success)
        {
            return match.Groups[1].Value;
        }

        return null;
   }
}
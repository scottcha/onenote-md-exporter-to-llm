using System;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

public static class HttpContentExtractor
{
    private static readonly HttpClient httpClient = new HttpClient();

    public static string GetHttpContent(string url)
    {
        try
        {
            var task = Task.Run(async () => await FetchHtmlContent(url));
            task.Wait();
            return CleanHtmlContent(task.Result);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching content from {url}: {ex.Message}");
            return null;
        }
    }

    private static string CleanHtmlContent(string input)
    {
        if (string.IsNullOrEmpty(input))
        {
            return string.Empty;
        }

        // Decode HTML entities
        string decoded = WebUtility.HtmlDecode(input);

        // Remove HTML tags
        string noHtml = Regex.Replace(decoded, "<.*?>", string.Empty);

        // Normalize whitespace
        string normalized = Regex.Replace(noHtml, @"\s+", " ").Trim();

        return normalized;
    }

    private static async Task<string> FetchHtmlContent(string url)
    {
        HttpResponseMessage response = await httpClient.GetAsync(url);
        response.EnsureSuccessStatusCode();
        string content = await response.Content.ReadAsStringAsync();
        return content;
    }
}
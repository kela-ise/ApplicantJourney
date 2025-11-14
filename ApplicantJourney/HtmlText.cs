using System;
using System.Net;
using HtmlAgilityPack;

/*
 Helper class for converting HTML descriptions to clean text.
 Uses Html Agility Pack to safely remove HTML tags and decode entities.
*/
namespace ApplicantJourney
{
    public static class HtmlText
    {
        public static string ToPlainText(string? html)
        {
            if (string.IsNullOrWhiteSpace(html))
                return string.Empty;

            var doc = new HtmlDocument();
            doc.LoadHtml(html);

            // Remove script and style elements that don't contain meaningful text
            var removable = doc.DocumentNode.SelectNodes("//script|//style");
            if (removable != null)
            {
                foreach (var node in removable)
                    node.Remove();
            }

            // Decode HTML entities and extract text content
            var text = WebUtility.HtmlDecode(doc.DocumentNode.InnerText ?? string.Empty);

            // Split into words and rejoin with single spaces for clean output
            var parts = text.Split((char[])null, StringSplitOptions.RemoveEmptyEntries);
            return string.Join(" ", parts).Trim();
        }

        public static string Preview(string? html, int maxChars)
        {
            var full = ToPlainText(html);
            if (string.IsNullOrEmpty(full)) return string.Empty;

            // Return truncated text with ellipsis if longer than max characters
            return full.Length <= maxChars ? full : full.Substring(0, maxChars) + "...";
        }
    }
}
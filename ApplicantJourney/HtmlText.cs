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

            var removable = doc.DocumentNode.SelectNodes("//script|//style");
            if (removable != null)
            {
                foreach (var node in removable)
                    node.Remove();
            }

            var text = WebUtility.HtmlDecode(doc.DocumentNode.InnerText ?? string.Empty);
            var parts = text.Split((char[])null, StringSplitOptions.RemoveEmptyEntries);
            return string.Join(" ", parts).Trim();
        }

        public static string Preview(string? html, int maxChars)
        {
            var full = ToPlainText(html);
            if (string.IsNullOrEmpty(full)) return string.Empty;
            return full.Length <= maxChars ? full : full.Substring(0, maxChars) + "...";
        }
    }
}

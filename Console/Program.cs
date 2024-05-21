using SharedModels.Models;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Xml;
using HtmlAgilityPack;

// URL of the website to parse
string url = "https://www.virgilfinance.com";

// Create HtmlWeb instance to load the webpage
HtmlWeb web = new HtmlWeb();

try
{
    // Load the webpage
    HtmlDocument document = web.Load(url);

    // Select all anchor tags in the document
    HtmlNodeCollection anchorNodes = document.DocumentNode.SelectNodes("//a[@href]");

    // List to store relative URLs
    List<string> relativeUrls = new List<string>();

    // Check each anchor tag for the href attribute
    if (anchorNodes != null)
    {
        foreach (HtmlNode anchorNode in anchorNodes)
        {
            // Get the href attribute value
            string hrefAttribute = anchorNode.GetAttributeValue("href", "");

            // Check if the href attribute is a relative URL
            if (!Uri.IsWellFormedUriString(hrefAttribute, UriKind.Absolute))
            {
                // Convert relative URL to absolute URL
                Uri baseUri = new Uri(url);
                Uri absoluteUri = new Uri(baseUri, hrefAttribute);

                // Add the relative URL to the list
                relativeUrls.Add(absoluteUri.ToString());
            }
        }

        // Print the relative URLs
        Console.WriteLine("Relative URLs:");
        foreach (string relativeUrl in relativeUrls)
        {
            Console.WriteLine(relativeUrl);
        }
    }
    else
    {
        Console.WriteLine("No anchor tags found in the document.");
    }
}
catch (Exception ex)
{
    Console.WriteLine($"Error: {ex.Message}");
}
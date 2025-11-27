using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using AngleSharp.Dom;
using BaskIt.Shared.DTOs.Product;
using BaskIt.Shared.Prompts.Scrape;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using OpenAI.Chat;

namespace BaskIt.Services.Scrape.ProductScraper.Strategies;

/// <summary>
/// AI-powered fallback strategy (lowest priority).
/// Only used when all structured data strategies fail.
/// </summary>
public class AiProductScraperStrategy : IProductScraperStrategy
{
    private readonly ChatClient chatClient;
    private readonly IConfiguration configuration;
    private readonly ILogger<AiProductScraperStrategy> logger;

    public int Priority => 5; // Lowest, for fallback

    public AiProductScraperStrategy(
        IConfiguration configuration,
        ILogger<AiProductScraperStrategy> logger,
        ChatClient chatClient)
    {
        this.configuration = configuration;
        this.logger = logger;
        this.chatClient = chatClient;
    }

    public bool CanHandle(IDocument document, string? url)
    {
        // Only handle if AI is enabled and there's enough content
        var aiEnabled = configuration.GetValue<bool>("ProductScraper:AI:Enabled", false);
        var hasContent = document.Body?.TextContent?.Length >= 100;

        return aiEnabled && hasContent;
    }

    public async Task<ProductScrapedDto?> Extract(IDocument document, string sourceUrl)
    {
        try
        {
            var cleanedHtml = CleanHtml(document);

            // Truncate if too large
            if (cleanedHtml.Length > 8000)
            {
                cleanedHtml = cleanedHtml.Substring(0, 8000);
                logger.LogWarning("HTML truncated to 8000 chars for AI extraction from {Url}", sourceUrl);
            }

            var result = await ExtractWithAiAsync(cleanedHtml, sourceUrl);
            return result;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "AI extraction failed for {Url}", sourceUrl);
            return null;
        }
    }


    private string CleanHtml(IDocument document)
    {
        // Clone document to avoid modifying original
        var clone = (IDocument)document.Clone();

        // Remove noise elements
        var noisySelectors = new[]
        {
            "script", "style", "svg", "noscript", "iframe",
            "header", "footer", "nav", "[role='navigation']",
            ".advertisement", ".cookie-banner", ".popup"
        };

        foreach (var selector in noisySelectors)
        {
            foreach (var element in clone.QuerySelectorAll(selector).ToList())
            {
                element.Remove();
            }
        }

        var mainContent = clone.QuerySelector("main")
                          ?? clone.QuerySelector("article")
                          ?? clone.QuerySelector("[role='main']")
                          ?? clone.QuerySelector(".product-detail")
                          ?? clone.QuerySelector(".product-container")
                          ?? clone.Body;

        return mainContent?.InnerHtml ?? string.Empty;
    }

    private async Task<ProductScrapedDto?> ExtractWithAiAsync(string html, string sourceUrl)
    {
        var prompt = ProductScraperPrompt.Template
            .Replace("{url}", sourceUrl)
            .Replace("{html}", html);

        var messages = new List<ChatMessage>
        {
            new SystemChatMessage(prompt)
        };

        var result = await chatClient.CompleteChatAsync(messages);
        var content = result.Value.Content[0].Text;

        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        var product = JsonSerializer.Deserialize<ProductScrapedDto>(content, options);

        if (product == null)
        {
            logger.LogWarning("AI returned invalid product data for {Url}", sourceUrl);
            return null;
        }

        return product;
    }
}

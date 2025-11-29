using System.Threading.RateLimiting;
using Microsoft.Playwright;

namespace BaskIt.Services.Scrape.WebFetcher;

public class WebContentFetcher : IWebContentFetcher
{
    private readonly RateLimiter rateLimiter;
    private IPlaywright? playwright;
    private IBrowser? browser;
    private readonly SemaphoreSlim browserLock = new(1, 1);

    public WebContentFetcher(RateLimiter rateLimiter)
    {
        this.rateLimiter = rateLimiter;
    }

    public async Task<string> FetchHtmlAsync(string url, CancellationToken ct = default)
    {
        using var lease = await this.rateLimiter.AcquireAsync(1, ct);

        if (!lease.IsAcquired)
        {
            throw new InvalidOperationException("Rate limit exceeded. Unable to fetch content.");
        }

        try
        {
            await this.EnsureBrowserInitializedAsync();

            var page = await this.browser!.NewPageAsync();

            try
            {
                var response = await page.GotoAsync(url, new PageGotoOptions
                {
                    WaitUntil = WaitUntilState.NetworkIdle
                });

                if (response == null || !response.Ok)
                {
                    throw new InvalidOperationException($"Failed to load page {url}. Status: {response?.Status}");
                }

                return await page.ContentAsync();
            }
            finally
            {
                await page.CloseAsync();
            }
        }
        catch (PlaywrightException ex)
        {
            throw new InvalidOperationException($"Failed to fetch HTML from {url} using Playwright", ex);
        }
    }

    private async Task EnsureBrowserInitializedAsync()
    {
        if (this.browser != null)
        {
            return;
        }

        await this.browserLock.WaitAsync();

        try
        {
            if (this.browser != null)
            {
                return;
            }

            this.playwright = await Playwright.CreateAsync();
            this.browser = await this.playwright.Chromium.LaunchAsync(new() { Headless = true });
        }
        finally
        {
            this.browserLock.Release();
        }
    }

    public async ValueTask DisposeAsync()
    {
        if (this.browser != null)
        {
            await this.browser.CloseAsync();
            await this.browser.DisposeAsync();
        }

        this.playwright?.Dispose();
        this.browserLock.Dispose();
    }
}

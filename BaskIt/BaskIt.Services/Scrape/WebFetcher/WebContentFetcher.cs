namespace BaskIt.Services.Scrape.WebFetcher;

public class WebContentFetcher : IWebContentFetcher
{


    public WebContentFetcher()
    {
        
    }

    public Task<string> FetchHtmlAsync(string url, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }
}

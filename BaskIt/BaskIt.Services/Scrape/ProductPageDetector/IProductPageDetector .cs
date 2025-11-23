namespace BaskIt.Services.Scrape.ProductPageDetector;

// IProductPageDetector performs lightweight product page detection for the /api/scrape/detect-product endpoint,
// allowing the Chrome extension to enable/disable itself without performing a full scrape.
// Instead of extracting all product details, it quickly checks for indicators like JSON-LD schema,
// OpenGraph meta tags, and common product page patterns (e.g., "Add to Cart" buttons, price elements).

// Really simple (just the fundamentals) example in the FE:

// chrome.tabs.onUpdated.addListener((tabId, changeInfo, tab) => {
//  if (changeInfo.status === 'complete' && tab.url) {
//    const criteriaMet = checkCriteria(tab.url); // Your logic here
//chrome.storage.local.set({ isExtensionActive: criteriaMet });
//  }
//});

//function checkCriteria(url)
//{
//    var isProduct = false; // call to the detector endpoint
//    return isProduct;
//}

public interface IProductPageDetector
{
    bool IsProductPage(string html);
}

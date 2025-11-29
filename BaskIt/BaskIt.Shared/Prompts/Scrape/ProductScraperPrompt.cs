namespace BaskIt.Shared.Prompts.Scrape;

public static class ProductScraperPrompt
{
    public const string Template = """
        Extract product information from the provided HTML and return ONLY a valid JSON object matching the exact structure specified below.

        URL: {url}

        HTML:
        {html}

        IMPORTANT: Return ONLY valid JSON with NO additional text, explanations, markdown formatting, or code blocks.

        Required JSON structure:
        {
          "name": "exact product name as string or null if not found",
          "price": numeric decimal value or 0 if not found,
          "websiteUrl": "use the URL provided above",
          "size": "product size as string or null if not found",
          "color": "product color as string or null if not found",
          "description": "product description as string or null if not found",
          "imageUrl": "main product image URL as string or null if not found"
        }

        Extraction Rules:
        1. "name": Extract the PRIMARY product name (NOT the website title, navigation text, or promotional text)
        2. "price": Extract ONLY the numeric value (remove currency symbols like $, €, £, commas, etc.). If multiple prices exist, choose the current/sale price.
        3. "websiteUrl": Use the exact URL provided at the top of this prompt
        4. "description": Extract the main product description (NOT reviews, specifications tables, or marketing copy)
        5. "color": Extract the currently selected/displayed color variant if available
        6. "size": Extract the currently selected/displayed size if available
        7. "imageUrl": Extract the main/primary product image URL (look for high-resolution product images, NOT icons or thumbnails)
        8. If any field cannot be reliably extracted, use null for strings or 0 for price
        9. Ensure all field names match EXACTLY (case-sensitive): name, price, websiteUrl, size, color, description, imageUrl

        OUTPUT FORMAT: Return ONLY the JSON object. Do not include:
        - Markdown code blocks (```json)
        - Explanatory text before or after the JSON
        - Comments within the JSON
        - Any other formatting

        Example valid response:
        {"name":"Nike Air Max 90","price":129.99,"websiteUrl":"https://example.com/product","size":"10","color":"Red","description":"Comfortable running shoes with air cushioning","imageUrl":"https://example.com/images/nike-air-max-90.jpg"}
        """;
}

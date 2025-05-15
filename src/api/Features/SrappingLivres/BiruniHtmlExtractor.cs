using HtmlAgilityPack;

namespace Features.ScrapingLivres;

public class BiruniHtmlExtractor : IDisposable
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<BiruniHtmlExtractor> _logger;

    public BiruniHtmlExtractor(HttpClient httpClient, ILogger<BiruniHtmlExtractor> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<List<RawBookData>> ExtractBooksAsync()
    {
        var books = new List<RawBookData>();
        int page = 1;
        bool hasNextPage;
        
        do
        {
            try
            {
                var response = await _httpClient.GetAsync($"catalogue-local.php?page={page}");
                if (!response.IsSuccessStatusCode) break;
                
                var doc = new HtmlDocument();
                doc.Load(await response.Content.ReadAsStreamAsync());
                
                var bookNodes = doc.DocumentNode.SelectNodes("//div[contains(@class, 'product-item')]");
                if (bookNodes == null) break;

                foreach (var node in bookNodes)
                {
                    var titleNode = node.SelectSingleNode(".//h3/a");
                    var title = titleNode?.InnerText.Trim() ?? string.Empty;
                    var url = titleNode?.GetAttributeValue("href", "") ?? string.Empty;
                    
                    // Extract additional details from the product page
                    var bookDetails = await ExtractBookDetails(url);
                    
                    books.Add(new RawBookData(
                        Title: title,
                        Url: url,
                        Author: bookDetails.Author,
                        Publisher: bookDetails.Publisher,
                        Year: bookDetails.Year,
                        ISBN: bookDetails.ISBN,
                        CoverImage: bookDetails.CoverImage,
                        Description: bookDetails.Description
                    ));
                }
                
                hasNextPage = doc.DocumentNode.SelectSingleNode("//a[contains(@class, 'next')]") != null;
                page++;
                await Task.Delay(2000); // Respectful delay between requests
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error extracting page {PageNumber}", page);
                break;
            }
        } while (hasNextPage);
        
        return books;
    }

    private async Task<BookDetails> ExtractBookDetails(string relativeUrl)
    {
        try
        {
            var response = await _httpClient.GetAsync(relativeUrl);
            if (!response.IsSuccessStatusCode) return new BookDetails();
            
            var doc = new HtmlDocument();
            doc.Load(await response.Content.ReadAsStreamAsync());
            
            // Extract details from the product page
            var detailsNode = doc.DocumentNode.SelectSingleNode("//div[contains(@class, 'product-details')]");
            
            return new BookDetails(
                Author: detailsNode?.SelectSingleNode(".//span[contains(text(), 'Auteur')]/following-sibling::text()")?.InnerText.Trim(),
                Publisher: detailsNode?.SelectSingleNode(".//span[contains(text(), 'Éditeur')]/following-sibling::text()")?.InnerText.Trim(),
                Year: detailsNode?.SelectSingleNode(".//span[contains(text(), 'Année')]/following-sibling::text()")?.InnerText.Trim(),
                ISBN: detailsNode?.SelectSingleNode(".//span[contains(text(), 'ISBN')]/following-sibling::text()")?.InnerText.Trim(),
                CoverImage: doc.DocumentNode.SelectSingleNode("//img[contains(@class, 'product-image')]")?.GetAttributeValue("src", ""),
                Description: doc.DocumentNode.SelectSingleNode("//div[contains(@class, 'product-description')]")?.InnerText.Trim()
            );
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error extracting book details from {Url}", relativeUrl);
            return new BookDetails();
        }
    }

    public void Dispose() => _httpClient.Dispose();
}

public record RawBookData(
    string Title,
    string Url,
    string? Author,
    string? Publisher,
    string? Year,
    string? ISBN,
    string? CoverImage,
    string? Description
);

public record BookDetails(
    string? Author = null,
    string? Publisher = null,
    string? Year = null,
    string? ISBN = null,
    string? CoverImage = null,
    string? Description = null
);


using domain.Entity;
using domain.Interfaces;

namespace Features.ScrapingLivres;

public class BookImportService
{
    private readonly BiruniHtmlExtractor _extractor;
    private readonly BookDataTransformer _transformer;
    private readonly IScrapingRepository _repository;
    private readonly ILogger<BookImportService> _logger;

    public BookImportService(
        BiruniHtmlExtractor extractor,
        BookDataTransformer transformer,
        IScrapingRepository repository,
        ILogger<BookImportService> logger)
    {
        _extractor = extractor;
        _transformer = transformer;
        _repository = repository;
        _logger = logger;
    }

    public async Task<ImportResult> ImportBooksAsync()
    {
        try
        {
            _logger.LogInformation("Starting book import process");
            
            // Extract data
            var rawBooks = await _extractor.ExtractBooksAsync();
            _logger.LogInformation("Extracted {Count} raw book records", rawBooks.Count);
            
            // Transform data
            var transformedBooks = new List<(Livres, Inventaire)>();
            foreach (var rawBook in rawBooks)
            {
                try
                {
                    transformedBooks.Add(_transformer.Transform(rawBook));
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "Failed to transform book: {Title}", rawBook.Title);
                }
            }
            
            // Load data
            var importedCount = await _repository.ImportBooksAsync(transformedBooks);
            
            _logger.LogInformation("Import completed. {Imported} new books added", importedCount);
            return new ImportResult(importedCount, transformedBooks.Count - importedCount);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Book import failed");
            throw;
        }
    }
}

public record ImportResult(int ImportedCount, int SkippedCount);
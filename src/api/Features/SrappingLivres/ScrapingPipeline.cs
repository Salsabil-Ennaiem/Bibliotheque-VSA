using domain.Interfaces;

namespace api.Features.ScrapingLivres;
public sealed class ScrapingPipeline{
    private readonly BiruniHtmlExtractor _extractor;
    private readonly BookDataTransformer _transformer;
    private readonly IScrapingRepository _repository;
    private readonly ILogger<ScrapingPipeline> _logger;

    public ScrapingPipeline(
        BiruniHtmlExtractor extractor,
        BookDataTransformer transformer,
        IScrapingRepository repository,
        ILogger<ScrapingPipeline> logger)
    {
        _extractor = extractor;
        _transformer = transformer;
        _repository = repository;
        _logger = logger;
    }

    public async Task<ScrapingResult> ExecuteAsync()
    {
        var nouveauxLivres = 0;
        var erreurs = 0;
        var rawData = await _extractor.ExtractBooksAsync();

        foreach (var raw in rawData)
        {
            try
            {
                var (livre, inventaire) = _transformer.Transform(raw);
                
                if (livre.isbn != null && !await _repository.LivreExistsAsync(livre.isbn))
                {
                    await _repository.AddLivreWithInventaireAsync(livre, inventaire);
                    nouveauxLivres++;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erreur sur le livre: {Titre}", raw.Title);
                erreurs++;
            }
        }

        return new ScrapingResult(nouveauxLivres, erreurs);
    }}public record ScrapingResult(int NouveauxLivres = 0, int Erreurs = 0);
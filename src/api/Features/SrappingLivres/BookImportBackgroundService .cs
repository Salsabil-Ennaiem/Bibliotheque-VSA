
namespace api.Features.ScrapingLivres;

public class BookImportBackgroundService : BackgroundService
{
    private readonly BookImportService _importService;
    private readonly ILogger<BookImportBackgroundService> _logger;
    private Timer? _timer;
    private bool _isRunning;

    public BookImportBackgroundService(
        BookImportService importService,
        ILogger<BookImportBackgroundService> logger)
    {
        _importService = importService;
        _logger = logger;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Book Import Background Service is starting");
        
        // Run every 6 hours
        _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromHours(6));
        
        return Task.CompletedTask;
    }

    private async void DoWork(object? state)
    {
        if (_isRunning) return;
        
        _isRunning = true;
        try
        {
            _logger.LogInformation("Starting scheduled book import");
            var result = await _importService.ImportBooksAsync();
            _logger.LogInformation("Scheduled import completed. Imported: {Imported}, Skipped: {Skipped}", 
                result.ImportedCount, result.SkippedCount);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in scheduled book import");
        }
        finally
        {
            _isRunning = false;
        }
    }

    public void TriggerRun()
    {
        DoWork(null);
    }

    public override async Task StopAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Book Import Background Service is stopping");
        _timer?.Change(Timeout.Infinite, 0);
        await base.StopAsync(stoppingToken);
    }

    public override void Dispose()
    {
        _timer?.Dispose();
        base.Dispose();
    }
}
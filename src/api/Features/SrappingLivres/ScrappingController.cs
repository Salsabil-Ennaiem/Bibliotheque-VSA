using Microsoft.AspNetCore.Mvc;

namespace Features.ScrapingLivres;

[ApiController]
[Route("api/import")]
public class ImportController : ControllerBase
{
    private readonly BookImportService _importService;
    private readonly ILogger<ImportController> _logger;

    public ImportController(
        BookImportService importService,
        ILogger<ImportController> logger)
    {
        _importService = importService;
        _logger = logger;
    }

    [HttpPost("books")]
    public async Task<IActionResult> ImportBooks()
    {
        try
        {
            _logger.LogInformation("Received request to import books");
            var result = await _importService.ImportBooksAsync();
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Book import failed");
            return StatusCode(500, "An error occurred during import");
        }
    }

    [HttpPost("schedule")]
    public IActionResult ScheduleImport([FromServices] IHostedService hostedService)
    {
        if (hostedService is BookImportBackgroundService backgroundService)
        {
            backgroundService.TriggerRun();
            return Ok("Import scheduled");
        }
        return BadRequest("Background service not available");
    }
}
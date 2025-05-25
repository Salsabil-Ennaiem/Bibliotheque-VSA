using Microsoft.AspNetCore.Mvc;

namespace api.Features.Sanction;

[ApiController]
[Route("api/[controller]")]
public class SanctionController : ControllerBase
{
    private readonly SanctionHandler _sanctionHandler;


    public SanctionController(SanctionHandler sanctionHandler)
    {
        _sanctionHandler = sanctionHandler;
    }

    [HttpGet("search")]
    public async Task<IActionResult> Search([FromQuery] string term)
    {
        var results = await _sanctionHandler.SearchAsync(term);
        return Ok(results);
    }

    [HttpGet("Getall")]
    public async Task<IActionResult> GetAll()
    {
        var sanctions = await _sanctionHandler.GetAllAsync();
        return Ok(sanctions);
    }
    
    [HttpPost("Create")]
    public async Task<IActionResult> Create([FromBody] CreateSanctionRequest createSanction)
    {
        var createdSanc = await _sanctionHandler.CreateAsync(createSanction);
        return Ok(createdSanc);
    }
    
}

using Microsoft.AspNetCore.Mvc;

namespace api.Features.Nouveautes;

[ApiController]
[Route("api/[controller]")]
public class NouveauteController : ControllerBase
{
    private readonly NouveauteHundler _nouveauteHandler;

    public NouveauteController(NouveauteHundler nouveauteHandler)
    {
        _nouveauteHandler = nouveauteHandler;
    }
    [HttpGet("Getall")]
    public async Task<IActionResult> GetAll()
    {
        var nouveautes = await _nouveauteHandler.GetAllAsync();
        return Ok(nouveautes);
    }
    [HttpGet("Get{id}")]
    public async Task<IActionResult> GetById(string id)
    {
        var nouveautes = await _nouveauteHandler.GetByIdAsync(id);
        return Ok(nouveautes);
    }
    [HttpPost("Create")]
    public async Task<IActionResult> Create([FromBody] CreateNouveauteRequest nouveautes)
    {
        var createdNouveaute = await _nouveauteHandler.CreateAsync(nouveautes);
        return Ok(createdNouveaute);
    }
    [HttpPut("Update{id}")]
    public async Task<IActionResult> Update(string id, [FromBody] CreateNouveauteRequest request)
    {
        var updated = await _nouveauteHandler.UpdateAsync(request, id);
        return Ok(updated);
    }
    [HttpDelete("Delete{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        await _nouveauteHandler.DeleteAsync(id);
        return NoContent();
    }
} 

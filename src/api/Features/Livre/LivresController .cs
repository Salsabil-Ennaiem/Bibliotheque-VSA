
using domain.Entity;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace api.Features.Livre;
[ApiController]
[Route("api/livres")]
public class LivresController : ControllerBase
{
    private readonly LivresHandler _livresHandler;


    public LivresController(LivresHandler livresHandler)
    {
        _livresHandler = livresHandler;
    }

    [HttpGet("search")]
    public async Task<IActionResult> Search([FromQuery] string term)
    {
        var results = await _livresHandler.SearchAsync(term);
        return Ok(results);
    }

    [HttpGet("Getall")]
    public async Task<IActionResult> GetAll()
    {
        var livres = await _livresHandler.GetAllAsync();
        return Ok(livres);
    }

    [HttpGet("Get{id}")]
    public async Task<IActionResult> GetById(string id)
    {
        var livre = await _livresHandler.GetByIdAsync(id);
        return Ok(livre);
    }

    [HttpPost("Create")]
    public async Task<IActionResult> Create([FromBody] CreateLivreRequest livre)
    {
        var createdLivre = await _livresHandler.CreateAsync(livre);
        return Ok(createdLivre);
    }

    [HttpPut("Update{id}")]
    public async Task<IActionResult> Update(string id, [FromBody] LivreDTO livre)
    {
        var updated = await _livresHandler.UpdateAsync(livre, id);
        return Ok(updated);
    }

    [HttpDelete("Delete{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        await _livresHandler.DeleteAsync(id);
        return NoContent();
    }


    [HttpPost("import")]
    public async Task<IActionResult> Import(IFormFile file)
    {
        if (file == null || file.Length == 0)
            return BadRequest("No file uploaded.");

        using var stream = file.OpenReadStream();
        await _livresHandler.ImportAsync(stream);
        return Ok("Import successful");
    }

    [HttpGet("export")]
    public async Task<IActionResult> Export()
    {
        var stream = await _livresHandler.ExportAsync();
        return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "LivresInventaire.xlsx");
    }
}
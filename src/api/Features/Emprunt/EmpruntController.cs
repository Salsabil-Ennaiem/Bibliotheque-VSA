using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api.Features.Emprunt;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class EmpruntController : ControllerBase
{
        private readonly EmpruntHandler _empruntHundler;


    public EmpruntController(EmpruntHandler empruntHundler)
    {
        _empruntHundler = empruntHundler;
    }

    [HttpGet("Notification")]
    public async Task<IActionResult> GetNotifications()
    {
       await _empruntHundler.NotifyOverdueEmpruntsAsync();
        return Ok("Notifications sent successfully.");
    }

    [HttpGet("search{term}")]
    public async Task<IActionResult> Search([FromQuery] string term)
    {
        var results = await _empruntHundler.SearchAsync(term);
        return Ok(results);
    }

    [HttpGet("Getall")]
    public async Task<IActionResult> GetAll()
    {
        var livres = await _empruntHundler.GetAllAsync();
        return Ok(livres);
    }

    [HttpGet("Get{id}")]
    public async Task<IActionResult> GetById(string id)
    {
        var livre = await _empruntHundler.GetByIdAsync(id);
        return Ok(livre);
    }

    [HttpPost("Create")]
    public async Task<IActionResult> Create([FromBody] CreateEmpRequest emp)
    {
        var createdLivre = await _empruntHundler.CreateAsync(emp);
        return Ok(createdLivre);
    }

    [HttpPut("Update{id}")]
    public async Task<IActionResult> Update(string id, [FromBody] UpdateEmppruntDTO emp)
    {
        var updated = await _empruntHundler.UpdateAsync(emp, id);
        return Ok(updated);
    }

    [HttpDelete("Delete{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        await _empruntHundler.DeleteAsync(id);
        return NoContent();
    }


    [HttpPost("import")]
    public async Task<IActionResult> Import(IFormFile file)
    {
        if (file == null || file.Length == 0)
            return BadRequest("No file uploaded.");

        using var stream = file.OpenReadStream();
        await _empruntHundler.ImportAsync(stream);
        return Ok("Import successful");
    }

    [HttpGet("export")]
    public async Task<IActionResult> Export()
    {
        var stream = await _empruntHundler.ExportAsync();
        return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Emprunts.xlsx");
    }
}

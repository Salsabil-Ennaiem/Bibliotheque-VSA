using Microsoft.AspNetCore.Mvc;

namespace api.Features.Parametre;

[ApiController]
[Route("api/[controller]")]
public class ParametreController : ControllerBase
{
    private readonly ParametreHandler _parametreHandler;
    public ParametreController(ParametreHandler parametreHandler)
    {
        _parametreHandler = parametreHandler;
    }
            [HttpGet("Get")]
        public async Task<IActionResult> GetById()
        {
            var Parametre = await _parametreHandler.GetByIdAsync();
            return Ok(Parametre);
        }
        [HttpPost("Update")]
        public async Task<IActionResult> Update([FromBody] ParametreDTO Parametre)
        {
            var CreateParametre = await _parametreHandler.CreateAsync(Parametre);
            return Ok(CreateParametre);
        }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace api.Features.Nouveautes
{

    [ApiController]
    [Route("api/[controller]")]
    public class NouveauteController : ControllerBase
    {
        private readonly NouveauteHandler _nouveauteHandler;

        public NouveauteController(NouveauteHandler nouveauteHandler)
        {
            _nouveauteHandler = nouveauteHandler;
        }
        [HttpGet("Getall")]
        public async Task<IActionResult> GetAllNouv()
        {
            var nouveautes = await _nouveauteHandler.GetAllNouvAsync();
            return Ok(nouveautes);
        }

        [Authorize]

        [HttpGet("GetallUser")]
        public async Task<IActionResult> GetAll()
        {
            var nouveautes = await _nouveauteHandler.GetAllAsync();
            return Ok(nouveautes);
        }

        [Authorize]

        [HttpGet("Get{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var nouveautes = await _nouveauteHandler.GetByIdAsync(id);
            return Ok(nouveautes);
        }
        [Authorize]

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromBody] CreateNouveauteRequest nouveautes)
        {
            var createdNouveaute = await _nouveauteHandler.CreateAsync(nouveautes);
            return Ok(createdNouveaute);
        }
        [Authorize]

        [HttpPut("Update{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] CreateNouveauteRequest request)
        {
            var updated = await _nouveauteHandler.UpdateAsync(request, id);
            return Ok(updated);
        }
        [Authorize]

        [HttpDelete("Delete{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            await _nouveauteHandler.DeleteAsync(id);
            return NoContent();
        }
    }
}
using Microsoft.AspNetCore.Mvc;

namespace api.Features.Profile;

[ApiController]
[Route("api/[controller]")]
public class ProfileController : ControllerBase
{

    private readonly ProfileHandler _profileHandler;

    public ProfileController(ProfileHandler profileHandler)
    {
        _profileHandler = profileHandler;
    }

    [HttpGet]
    public async Task<ActionResult<ProfileDTO>> GetProfile(CancellationToken cancellationToken)
    {
        var profile = await _profileHandler.GetProfileAsync(cancellationToken);
        return Ok(profile);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileDto dto, CancellationToken cancellationToken)
    {
        await _profileHandler.UpdateProfileAsync(dto, cancellationToken);
        return NoContent();
    }
}


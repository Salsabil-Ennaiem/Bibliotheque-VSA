using Microsoft.AspNetCore.Mvc;

namespace api.Features.Auth.Login;

[ApiController]
[Route("api/[controller]")]
public class LoginController : ControllerBase
{
    private readonly LoginHandler _loginHandler;
    public LoginController(LoginHandler loginHandler)
    {
        _loginHandler = loginHandler;
    }
         [HttpPost]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
    {
        var command = new LoginCommand(request.Email, request.Password);
        var response = await _loginHandler.LoginAsync(command);
        return Ok(response);
    }
}

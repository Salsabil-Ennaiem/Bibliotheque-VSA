
namespace api.Features.Auth.Login
{
    public record LoginCommand(string Email, string Password);

    public class LoginRequestDto
    {
        public required string Email { get; set; }
        public required string Password { get; set; }
    }

    public class LoginResponseDto
    {
        public required string Token { get; set; }
    }
}

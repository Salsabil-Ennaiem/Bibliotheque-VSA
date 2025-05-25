
namespace api.Features.Auth.Login
{
    // Command with request properties
    public record LoginCommand(string Email, string Password);

    // Request DTO (optional, can be used in controller)
    public class LoginRequestDto
    {
        public required string Email { get; set; }
        public required string Password { get; set; }
    }

    // Response DTO
    public class LoginResponseDto
    {
        public required string Token { get; set; }
    }
}

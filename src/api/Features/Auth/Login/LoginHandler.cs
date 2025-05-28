using domain.Entity;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace api.Features.Auth.Login
{
    public class LoginHandler 
    {
        private readonly UserManager<Bibliothecaire> _userManager;
        private readonly SignInManager<Bibliothecaire> _signInManager; 
        private readonly IConfiguration _configuration;

        public LoginHandler(
            UserManager<Bibliothecaire> userManager,
            SignInManager<Bibliothecaire> signInManager,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager ;
            _configuration = configuration ;
        }

        public async Task<LoginResponseDto> LoginAsync(LoginCommand request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Password))
                throw new UnauthorizedAccessException("Email and password are required");

            try
            {
                var user = await _userManager.FindByEmailAsync(request.Email);
                if (user == null)
                    throw new UnauthorizedAccessException("Invalid credentials");

                if (await _userManager.IsLockedOutAsync(user))
                    throw new UnauthorizedAccessException("Account is locked. Please contact an administrator.");

                if (_userManager.Options.SignIn.RequireConfirmedEmail && !await _userManager.IsEmailConfirmedAsync(user))
                    throw new UnauthorizedAccessException("Email not confirmed. Please check your email inbox.");

                var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, true); // Enable lockout
                if (!result.Succeeded)
                {
                    if (result.IsLockedOut)
                        throw new UnauthorizedAccessException("Account is locked due to multiple failed attempts. Please try again later.");
                    
                    throw new UnauthorizedAccessException("Invalid credentials");
                }

                var token = GenerateJwtToken(user);
                var userRoles = await _userManager.GetRolesAsync(user);
                return new LoginResponseDto { Token = token };
            }
            catch (UnauthorizedAccessException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred during login. Please try again later.", ex);
            }
        }

        private string GenerateJwtToken(Bibliothecaire user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            try
            {
                var jwtSecret = _configuration["Jwt:Secret"];
                var jwtIssuer = _configuration["Jwt:ValidIssuer"];
                var jwtAudience = _configuration["Jwt:ValidAudience"];
                var jwtExpiryHours = _configuration.GetValue<int>("Jwt:ExpiryHours", 1); 

                if (string.IsNullOrEmpty(jwtSecret))
                    throw new InvalidOperationException("JWT Secret is not configured");
                if (string.IsNullOrEmpty(jwtIssuer))
                    throw new InvalidOperationException("JWT Issuer is not configured");
                if (string.IsNullOrEmpty(jwtAudience))
                    throw new InvalidOperationException("JWT Audience is not configured");

                // Get user roles
                var userRoles = _userManager.GetRolesAsync(user).Result;

                var claims = new List<Claim>
                {
                    new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                    new Claim(JwtRegisteredClaimNames.Email, user.Email ?? ""),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                };

                foreach (var role in userRoles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, role));
                }

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(
                    issuer: jwtIssuer,
                    audience: jwtAudience,
                    claims: claims,
                    expires: DateTime.UtcNow.AddHours(jwtExpiryHours),
                    signingCredentials: creds);

                return new JwtSecurityTokenHandler().WriteToken(token);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Failed to generate authentication token. Please contact support.", ex);
            }
        }
    }
}
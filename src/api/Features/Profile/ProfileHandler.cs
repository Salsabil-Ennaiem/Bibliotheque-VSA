using System.Security.Claims;
using domain.Entity;
using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace api.Features.Profile;

public class ProfileHandler
{
    private readonly UserManager<Bibliothecaire> _userManager;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ILogger<ProfileHandler> _logger;

    public ProfileHandler(
        UserManager<Bibliothecaire> userManager,
        IHttpContextAccessor httpContextAccessor,
        ILogger<ProfileHandler> logger)
    {
        _userManager = userManager;
        _httpContextAccessor = httpContextAccessor;
        _logger = logger;
    }

    public async Task<ProfileDTO> GetCurrentUserAsync(CancellationToken cancellationToken)
    {
        try
        {
            var userId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                _logger.LogWarning("GetCurrentUserAsync: User is not authenticated.");
                throw new UnauthorizedAccessException("User is not authenticated.");
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                _logger.LogWarning("GetCurrentUserAsync: User not found. UserId: {UserId}", userId);
                throw new UnauthorizedAccessException("User not found.");
            }

            cancellationToken.ThrowIfCancellationRequested();

            return user.Adapt<ProfileDTO>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in GetCurrentUserAsync");
            throw;
        }
    }

    public async Task<ProfileDTO> GetProfileAsync(CancellationToken cancellationToken)
    {
        try
        {
            var user = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext!.User);
            if (user == null)
            {
                _logger.LogWarning("GetProfileAsync: User is not authenticated.");
                throw new UnauthorizedAccessException("User is not authenticated.");
            }

            return user.Adapt<ProfileDTO>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in GetProfileAsync");
            throw;
        }
    }

    public async Task UpdateProfileAsync(UpdateProfileDto dto, CancellationToken cancellationToken)
    {
        try
        {
            var user = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext!.User);
            if (user == null)
            {
                _logger.LogWarning("UpdateProfileAsync: User is not authenticated.");
                throw new UnauthorizedAccessException("User is not authenticated.");
            }

            // Update fields
            user.nom = dto.Nom ?? user.nom;
            user.prenom = dto.Prenom ?? user.prenom;
            user.PhoneNumber = dto.Telephone ?? user.PhoneNumber;

            if (!string.IsNullOrEmpty(dto.Email) && dto.Email != user.Email)
            {
                var setEmailResult = await _userManager.SetEmailAsync(user, dto.Email);
                if (!setEmailResult.Succeeded)
                {
                    var error = setEmailResult.Errors.FirstOrDefault()?.Description ?? "Failed to update email.";
                    _logger.LogWarning("UpdateProfileAsync: Failed to update email for user {UserId}. Error: {Error}", user.Id, error);
                    throw new InvalidOperationException(error);
                }
            }

            if (!string.IsNullOrEmpty(dto.NouveauMotDePasse))
            {
                if (string.IsNullOrEmpty(dto.AncienMotDePasse))
                {
                    _logger.LogWarning("UpdateProfileAsync: AncienMotDePasse is required to change password for user {UserId}.", user.Id);
                    throw new ArgumentException("AncienMotDePasse is required to change password.");
                }

                var passwordChangeResult = await _userManager.ChangePasswordAsync(user, dto.AncienMotDePasse, dto.NouveauMotDePasse);
                if (!passwordChangeResult.Succeeded)
                {
                    var error = passwordChangeResult.Errors.FirstOrDefault()?.Description ?? "Failed to change password.";
                    _logger.LogWarning("UpdateProfileAsync: Failed to change password for user {UserId}. Error: {Error}", user.Id, error);
                    throw new InvalidOperationException(error);
                }
            }

            var updateResult = await _userManager.UpdateAsync(user);
            if (!updateResult.Succeeded)
            {
                var error = updateResult.Errors.FirstOrDefault()?.Description ?? "Failed to update profile.";
                _logger.LogWarning("UpdateProfileAsync: Failed to update profile for user {UserId}. Error: {Error}", user.Id, error);
                throw new InvalidOperationException(error);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in UpdateProfileAsync");
            throw;
        }
    }
}

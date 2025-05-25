using domain.Entity;
using Microsoft.AspNetCore.Identity;
namespace api.Features.Auth.RestPasseword;

public sealed record ResetPasswordCommand(string Email, string Token, string NewPassword);
public sealed class ResetPasswordHandler(UserManager<Bibliothecaire> userManager)
{
    public async Task<IResult> Handle(ResetPasswordCommand request)
    {
        var user = await userManager.FindByEmailAsync(request.Email);
        if (user is null) return TypedResults.BadRequest("Invalid request");
        
        var result = await userManager.ResetPasswordAsync(
            user, request.Token, request.NewPassword);
        
        return result.Succeeded 
            ? TypedResults.Ok() 
            : TypedResults.BadRequest(result.Errors);
    }
}
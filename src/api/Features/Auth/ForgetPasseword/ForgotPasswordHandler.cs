using domain.Entity;
using Microsoft.AspNetCore.Identity;
using MailKit.Net.Smtp;
using MimeKit;
using MailKit.Security;

namespace api.Features.Auth.ForgetPassword;
public sealed record ForgotPasswordCommand(string Email);

public sealed class ForgotPasswordHandler
{

    private readonly UserManager<Bibliothecaire> _userManager;
    private readonly IConfiguration _config;

    public ForgotPasswordHandler(UserManager<Bibliothecaire> userManager, IConfiguration config)
    {
        _userManager = userManager;
        _config = config;
    }
    public async Task<IResult> Handle(ForgotPasswordCommand request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user is null) return TypedResults.Ok();

        var token = await _userManager.GeneratePasswordResetTokenAsync(user);
        var resetLink = $"{_config["ClientUrl"]}/reset-password?token={token}&email={user.Email}";

        await SendResetEmail(user.Email!, resetLink);
        return TypedResults.Ok();
    }

    private async Task SendResetEmail(string email, string resetLink)
    {
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress(
            _config["MailSettings:DisplayName"],
            _config["MailSettings:From"]));
        message.To.Add(new MailboxAddress("", email));
        message.Subject = "Password Reset";
        message.Body = new TextPart("html")
        {
            Text = $"Click <a href='{resetLink}'>here</a> to reset your password."
        };

        using var client = new SmtpClient();
        await client.ConnectAsync(
            _config["MailSettings:Host"],
            _config.GetValue<int>("MailSettings:Port"),
            SecureSocketOptions.StartTls);

        await client.AuthenticateAsync(
            _config["MailSettings:Username"],
            _config["MailSettings:Password"]);

        await client.SendAsync(message);
        await client.DisconnectAsync(true);
    }
}
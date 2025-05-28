using domain.Entity;
using Microsoft.AspNetCore.Identity;
using MailKit.Net.Smtp;
using MimeKit;
using MailKit.Security;
using System.Web;

namespace api.Features.Auth.ForgetPassword;

public sealed record ForgotPasswordCommand(string Email);
public sealed record ResetPasswordCommand(string Email, string Token, string NewPassword);

public sealed class ForgotPasswordHandler
{
    private readonly UserManager<Bibliothecaire> _userManager;
    private readonly IConfiguration _config;
    private readonly ILogger<ForgotPasswordHandler> _logger;

    public ForgotPasswordHandler(
        UserManager<Bibliothecaire> userManager, 
        IConfiguration config,
        ILogger<ForgotPasswordHandler> logger)
    {
        _userManager = userManager;
        _config = config;
        _logger = logger;
    }

    public async Task<IResult> Handle(ForgotPasswordCommand request)
    {
        if (string.IsNullOrWhiteSpace(request.Email))
        {
            return TypedResults.BadRequest(new { message = "Email is required" });
        }

        if (!IsValidEmail(request.Email))
        {
            return TypedResults.BadRequest(new { message = "Invalid email format" });
        }

        try
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                _logger.LogWarning("Password reset requested for non-existent email: {Email}", request.Email);
                // Return success message for security (don't reveal if email exists)
                return TypedResults.Ok(new { message = "If the email exists, a password reset link has been sent." });
            }

            // Check if user account is locked
            if (await _userManager.IsLockedOutAsync(user))
            {
                _logger.LogWarning("Password reset requested for locked account: {Email}", request.Email);
                return TypedResults.BadRequest(new { message = "Account is locked. Please contact an administrator." });
            }

            // Validate mail configuration before proceeding
            if (!IsMailConfigurationValid())
            {
                _logger.LogError("Mail configuration is invalid or missing");
                return TypedResults.Problem("Email service is currently unavailable. Please try again later.");
            }

            // Generate password reset token
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var encodedToken = HttpUtility.UrlEncode(token);
            var encodedEmail = HttpUtility.UrlEncode(user.Email!);
            var resetLink = $"{_config["ClientUrl"]}/reset-password?token={encodedToken}&email={encodedEmail}";

            // Send reset email
            await SendResetEmail(user.Email!, resetLink, user.UserName ?? "User");
            
            _logger.LogInformation("Password reset email sent to: {Email}", request.Email);
            return TypedResults.Ok(new { message = "If the email exists, a password reset link has been sent." });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while processing forgot password request for email: {Email}", request.Email);
            return TypedResults.Problem("An error occurred while processing your request. Please try again later.");
        }
    }

    public async Task<IResult> HandleResetPassword(ResetPasswordCommand request)
    {
        // Validate input
        if (string.IsNullOrWhiteSpace(request.Email) || 
            string.IsNullOrWhiteSpace(request.Token) || 
            string.IsNullOrWhiteSpace(request.NewPassword))
        {
            return TypedResults.BadRequest(new { message = "All fields are required" });
        }

        // Validate email format
        if (!IsValidEmail(request.Email))
        {
            return TypedResults.BadRequest(new { message = "Invalid email format" });
        }

        try
        {
            // Check if user exists FIRST
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                _logger.LogWarning("Password reset attempted for non-existent email: {Email}", request.Email);
                return TypedResults.BadRequest(new { message = "Invalid reset request" });
            }

            // Check if user account is locked
            if (await _userManager.IsLockedOutAsync(user))
            {
                _logger.LogWarning("Password reset attempted for locked account: {Email}", request.Email);
                return TypedResults.BadRequest(new { message = "Account is locked. Please contact an administrator." });
            }

            // Validate password strength
            if (request.NewPassword.Length < 6)
            {
                return TypedResults.BadRequest(new { message = "Password must be at least 6 characters long" });
            }

            // Reset the password using the token
            var result = await _userManager.ResetPasswordAsync(user, request.Token, request.NewPassword);
            
            if (result.Succeeded)
            {
                // Reset failed login attempts
                await _userManager.ResetAccessFailedCountAsync(user);
                _logger.LogInformation("Password successfully reset for user: {Email}", request.Email);
                
                // Send confirmation email
                try
                {
                    if (IsMailConfigurationValid())
                    {
                        await SendPasswordResetConfirmationEmail(user.Email!, user.UserName ?? "User");
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "Failed to send confirmation email, but password reset was successful for: {Email}", request.Email);
                }
                
                return TypedResults.Ok(new { message = "Password has been reset successfully", success = true });
            }
            else
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                _logger.LogWarning("Password reset failed for user {Email}. Errors: {Errors}", request.Email, errors);
                return TypedResults.BadRequest(new { message = $"Password reset failed: {errors}", success = false });
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while resetting password for email: {Email}", request.Email);
            return TypedResults.Problem("An error occurred while resetting your password. Please try again later.");
        }
    }

    private bool IsValidEmail(string email)
    {
        try
        {
            var addr = new System.Net.Mail.MailAddress(email);
            return addr.Address == email;
        }
        catch
        {
            return false;
        }
    }

    private bool IsMailConfigurationValid()
    {
        var host = _config["MailSettings:Host"];
        var username = _config["MailSettings:Username"];
        var password = _config["MailSettings:Password"];
        var from = _config["MailSettings:From"];
        var port = _config.GetValue<int>("MailSettings:Port");

        return !string.IsNullOrWhiteSpace(host) &&
               !string.IsNullOrWhiteSpace(username) &&
               !string.IsNullOrWhiteSpace(password) &&
               !string.IsNullOrWhiteSpace(from) &&
               port > 0;
    }

    private async Task SendResetEmail(string email, string resetLink, string userName)
    {
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress(
            _config["MailSettings:DisplayName"] ?? "Bibliotheque VSA",
            _config["MailSettings:From"]));
        message.To.Add(new MailboxAddress(userName, email));
        message.Subject = "Password Reset - Bibliotheque VSA";

        var htmlBody = $@"
            <html>
            <body style='font-family: Arial, sans-serif; line-height: 1.6; color: #333;'>
                <div style='max-width: 600px; margin: 0 auto; padding: 20px;'>
                    <h2 style='color: #2c3e50;'>Password Reset Request</h2>
                    <p>Hello {userName},</p>
                    <p>We received a request to reset your password for your Bibliotheque VSA account.</p>
                    <p>Click the button below to reset your password:</p>
                    <div style='text-align: center; margin: 30px 0;'>
                        <a href='{resetLink}' 
                           style='background-color: #3498db; color: white; padding: 12px 30px; 
                                  text-decoration: none; border-radius: 5px; display: inline-block;'>
                            Reset Password
                        </a>
                    </div>
                    <p>If the button doesn't work, you can copy and paste this link into your browser:</p>
                    <p style='word-break: break-all; color: #7f8c8d;'>{resetLink}</p>
                    <p><strong>Important:</strong> This link will expire in 1 hour for security reasons.</p>
                    <p>If you didn't request this password reset, please ignore this email or contact support if you have concerns.</p>
                    <hr style='border: none; border-top: 1px solid #eee; margin: 30px 0;'>
                    <p style='font-size: 12px; color: #7f8c8d;'>
                        This is an automated message from Bibliotheque VSA. Please do not reply to this email.
                    </p>
                </div>
            </body>
            </html>";

        message.Body = new TextPart("html") { Text = htmlBody };

        using var client = new SmtpClient();
        
        // Connect with timeout
        var cancellationToken = new CancellationTokenSource(TimeSpan.FromSeconds(30)).Token;
        
        await client.ConnectAsync(
            _config["MailSettings:Host"],
            _config.GetValue<int>("MailSettings:Port"),
            SecureSocketOptions.StartTls,
            cancellationToken);

        await client.AuthenticateAsync(
            _config["MailSettings:Username"],
            _config["MailSettings:Password"],
            cancellationToken);

        await client.SendAsync(message, cancellationToken);
        await client.DisconnectAsync(true, cancellationToken);
        
        _logger.LogInformation("Password reset email sent successfully to: {Email}", email);
    }

    private async Task SendPasswordResetConfirmationEmail(string email, string userName)
    {
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress(
            _config["MailSettings:DisplayName"] ?? "Bibliotheque VSA",
            _config["MailSettings:From"]));
        message.To.Add(new MailboxAddress(userName, email));
        message.Subject = "Password Reset Confirmation - Bibliotheque VSA";

        var htmlBody = $@"
            <html>
            <body style='font-family: Arial, sans-serif; line-height: 1.6; color: #333;'>
                <div style='max-width: 600px; margin: 0 auto; padding: 20px;'>
                    <h2 style='color: #27ae60;'>Password Reset Successful</h2>
                    <p>Hello {userName},</p>
                    <p>Your password has been successfully reset for your Bibliotheque VSA account.</p>
                    <p>If you did not make this change, please contact our support team immediately.</p>
                    <p>For security reasons, we recommend:</p>
                    <ul>
                        <li>Using a strong, unique password</li>
                        <li>Not sharing your password with anyone</li>
                        <li>Logging out from shared devices</li>
                    </ul>
                    <hr style='border: none; border-top: 1px solid #eee; margin: 30px 0;'>
                    <p style='font-size: 12px; color: #7f8c8d;'>
                        This is an automated message from Bibliotheque VSA. Please do not reply to this email.
                    </p>
                </div>
            </body>
            </html>";

        message.Body = new TextPart("html") { Text = htmlBody };

        using var client = new SmtpClient();
        
        var cancellationToken = new CancellationTokenSource(TimeSpan.FromSeconds(30)).Token;
        
        await client.ConnectAsync(
            _config["MailSettings:Host"],
            _config.GetValue<int>("MailSettings:Port"),
            SecureSocketOptions.StartTls,
            cancellationToken);

        await client.AuthenticateAsync(
            _config["MailSettings:Username"],
            _config["MailSettings:Password"],
            cancellationToken);

        await client.SendAsync(message, cancellationToken);
        await client.DisconnectAsync(true, cancellationToken);
    }
}

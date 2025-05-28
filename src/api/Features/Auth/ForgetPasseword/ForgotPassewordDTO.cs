namespace api.Features.Auth.ForgetPassword;

public sealed record ForgotPasswordRequestDto(string Email);

public sealed record ForgotPasswordResponseDto(string Message);

public sealed record ResetPasswordRequestDto(string Email, string Token, string NewPassword);

public sealed record ResetPasswordResponseDto(string Message, bool Success);

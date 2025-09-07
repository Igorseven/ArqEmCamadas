namespace ArqEmCamadas.ApplicationService.Dtos.UserDtos.Request;

public sealed record ResetPasswordRequest
{
    public required string Token { get; init; }
    public required string Email { get; init; }
    public required string NewPassword { get; init; }
    public required string ConfirmPassword { get; init; }
}
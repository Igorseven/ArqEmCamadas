namespace ArqEmCamadas.ApplicationService.Dtos.AuthenticationDtos.Request;

public sealed record UserChangePasswordRequest
{
    public required Guid UserId { get; init; }
    public required string NewPassword { get; init; }
}
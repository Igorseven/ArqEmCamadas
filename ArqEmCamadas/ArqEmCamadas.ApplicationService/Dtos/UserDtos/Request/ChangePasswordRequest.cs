namespace ArqEmCamadas.ApplicationService.Dtos.UserDtos.Request;

public sealed record ChangePasswordRequest
{
    public Guid UserId { get; init; }
    public required string NewPassword { get; init; }
}
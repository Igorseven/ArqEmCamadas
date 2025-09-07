namespace ArqEmCamadas.ApplicationService.Dtos.AuthenticationDtos.Request;

public sealed record UserLoginRequest
{
    public required string Login { get; set; }
    public string? Password { get; init; }
    public required Guid SystemOrigin { get; init; }
}
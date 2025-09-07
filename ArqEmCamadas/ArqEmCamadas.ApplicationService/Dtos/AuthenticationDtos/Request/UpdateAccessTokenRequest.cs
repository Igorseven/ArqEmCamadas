namespace ArqEmCamadas.ApplicationService.Dtos.AuthenticationDtos.Request;

public sealed record UpdateAccessTokenRequest
{
    public required string AccessToken { get; init; }
    public required string RefreshToken { get; init; }
    public required Guid SystemOrigin { get; init; }
}
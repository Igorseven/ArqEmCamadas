namespace ArqEmCamadas.ApplicationService.Dtos.AuthenticationDtos.Response;

public sealed record AuthenticationLoginResponse
{
    public required string AccessToken { get; init; }
    public required string RefreshToken { get; init; }
    public required double Expiry { get; init; }
}
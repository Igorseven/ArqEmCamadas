using Microsoft.IdentityModel.Tokens;

namespace ArqEmCamadas.ApplicationService.Dtos.AuthenticationDtos.Request;

public sealed record ExtractUserRequest
{
    public required Guid UserId { get; init; }
    public required string AccessToken { get; init; }
    public required string SecurityAlgorithm { get; init; }
    public required TokenValidationParameters TokenValidationParameters { get; init; }
}
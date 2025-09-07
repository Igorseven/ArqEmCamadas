namespace ArqEmCamadas.ApplicationService.Dtos.UserDtos.Request;

public sealed record UserRegisterRequest
{
    public required string Name { get; init; }
    public required string Email { get; init; }
    public required string Password { get; init; }
}
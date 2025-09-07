namespace ArqEmCamadas.ApplicationService.Dtos.UserDtos.Request;

public sealed record UserFacadeRegisterRequest
{
    public required string Name { get; init; }
    public required string Email { get; init; }
    public required string Document { get; init; }
    public required string Password { get; init; }
    public Guid Owner { get; init; }
}
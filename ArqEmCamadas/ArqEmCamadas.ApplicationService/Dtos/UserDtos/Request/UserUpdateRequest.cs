namespace ArqEmCamadas.ApplicationService.Dtos.UserDtos.Request;

public sealed record UserUpdateRequest
{
    public Guid Id { get; init; }
    public required string Name { get; init; }
}
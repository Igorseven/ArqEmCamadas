namespace ArqEmCamadas.Domain.ValueObjects;

public sealed record UserCredential
{
    public Guid Id { get; init; }
    public required List<string> Roles { get; set; }
}
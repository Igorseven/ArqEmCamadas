namespace ArqEmCamadas.Domain.ValueObjects;

public sealed record UserFullData
{
    public Guid Id { get; init; }
    public Guid CompanyId { get; init; }
    public required string Name { get; init; }
    public required List<string> Roles { get; init; }
}
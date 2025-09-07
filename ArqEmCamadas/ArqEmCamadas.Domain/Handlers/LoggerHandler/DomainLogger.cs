namespace ArqEmCamadas.Domain.Handlers.LoggerHandler;

public sealed class DomainLogger
{
    public long Id { get; init; }
    public EUserAction Action { get; init; }
    public DateTime ActionDate { get; init; }
    public required string Description { get; init; }
    public Guid UserId { get; init; }
    public string? EntityId { get; init; }
}
namespace ArqEmCamadas.Domain.Providers;

public sealed class ConnectionStringOptions
{
    public const string SectionName = "ConnectionStrings";
    public required string DefaultConnection { get; init; }
}

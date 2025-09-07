namespace ArqEmCamadas.Domain.Providers;

public sealed class EmailSettingsOptions
{
    public const string SectionName = "EmailSettings";
    
    public required string SenderName { get; init; }
    public required string SenderEmail { get; init; }
    public required string Password { get; init; }
    public required string ServerAddress { get; init; }
    public required int PortSslOrTls { get; init; }
    public required int PortStartTls { get; init; }
    public required bool Ssl { get; init; }
    public required bool Active { get; init; }
}
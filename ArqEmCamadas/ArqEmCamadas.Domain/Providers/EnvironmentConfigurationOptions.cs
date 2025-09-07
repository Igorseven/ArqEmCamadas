namespace ArqEmCamadas.Domain.Providers;

public sealed class EnvironmentConfigurationOptions
{
    public const string SectionName = "EnvironmentConfiguration";
    public required bool ActiveSwagger { get; init; }
}
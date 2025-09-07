namespace ArqEmCamadas.Domain.Providers;

public sealed class CorsConfigurationOptions
{
    public const string SectionName = "CorsConfiguration";
    public required string Web { get; set; }
    public required string[] Methods { get; set; }
}
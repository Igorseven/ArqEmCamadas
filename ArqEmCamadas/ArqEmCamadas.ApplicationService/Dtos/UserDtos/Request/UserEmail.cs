namespace ArqEmCamadas.ApplicationService.Dtos.UserDtos.Request;

public sealed record UserEmail
{
    public string ToEmail { get; set; } = string.Empty;
    public string Subject { get; set; } = string.Empty;
    public string Body { get; set; } = string.Empty;
}
namespace ArqEmCamadas.ApplicationService.Dtos.EmailDtos.Request;

public sealed record EmailSenderRequest
{
    public string? ClientSideUrl { get; set; }
    public required string Date { get; init; }
    public string? UserName { get; init; }
    public string? Password { get; init; }
    public required string Email { get; init; }
    public required string Description { get; init; }
}
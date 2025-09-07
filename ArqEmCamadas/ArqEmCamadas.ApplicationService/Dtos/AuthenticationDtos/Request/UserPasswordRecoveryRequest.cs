namespace ArqEmCamadas.ApplicationService.Dtos.AuthenticationDtos.Request;

public sealed record UserPasswordRecoveryRequest
{
    public required string Email { get; init; } 
}
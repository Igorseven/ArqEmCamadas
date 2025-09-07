namespace ArqEmCamadas.ApplicationService.Dtos.UserDtos.Response;

public class UserPaginationResponse
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public required string Email { get; set; }
    public required DateTime RegistrationDate { get; set; }
    public required bool Status { get; set; }
}

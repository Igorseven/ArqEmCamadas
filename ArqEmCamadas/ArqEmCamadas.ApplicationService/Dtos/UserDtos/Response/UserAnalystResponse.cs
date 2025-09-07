using ArqEmCamadas.Domain.Enum;

namespace ArqEmCamadas.ApplicationService.Dtos.UserDtos.Response;

public class UserAnalystResponse
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public required string Email { get; set; }
    public required EUserStatus Status { get; set; }

}

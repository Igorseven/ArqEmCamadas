using ArqEmCamadas.Domain.Enum;
using Microsoft.AspNetCore.Identity;

namespace ArqEmCamadas.Domain.Entities;

public sealed class User : IdentityUser<Guid>
{
    public required string Name { get; set; }
    public DateTime RegistrationDate { get; set; }
    public EUserStatus Status { get; set; }
    
    
    public List<UserRole>? UserRoles { get; set; }
    public List<UserClaim>? UserClaims { get; set; }
    public List<UserToken>? UserTokens { get; set; }
}
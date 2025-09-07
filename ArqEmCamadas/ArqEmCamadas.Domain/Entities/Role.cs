using Microsoft.AspNetCore.Identity;

namespace ArqEmCamadas.Domain.Entities;

public sealed class Role : IdentityRole<Guid>
{
    public bool Active { get; set; }
    
    public List<UserRole>? UserRoles { get; set; }
    public List<RoleClaim>? RoleClaims { get; set; }
}
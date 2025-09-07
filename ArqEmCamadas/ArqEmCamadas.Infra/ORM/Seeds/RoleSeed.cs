using ArqEmCamadas.Domain.Entities;

namespace ArqEmCamadas.Infra.ORM.Seeds;

public static class RoleSeed
{
    public static List<Role> CreateRoles() =>
    [
        new()
        {
            Id = new Guid("734b18b5-1291-4ce8-943d-1356479306da"),
            Name = "Administrator",
            NormalizedName = "ADMINISTRATOR",
            Active = true
        },
        new()
        {
            Id = new Guid("992e4816-ed92-4a83-9b12-7903001d3ca0"),
            Name = "Analyst",
            NormalizedName = "ANALYST",
            Active = true
        },
        new()
        {
            Id = new Guid("52d5c576-9aed-4e97-ba68-89b3b3a913f7"),
            Name = "System",
            NormalizedName = "SYSTEM",
            Active = true
        }
    ];
}
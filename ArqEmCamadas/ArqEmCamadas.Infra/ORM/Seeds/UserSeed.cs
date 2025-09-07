using ArqEmCamadas.Domain.Entities;
using ArqEmCamadas.Domain.Enum;

namespace ArqEmCamadas.Infra.ORM.Seeds;

public static class UserSeed
{
    public static List<User> CreateUserSeed() =>
    [
        new User
        {
            Name = "Igor Seven",
            UserName = "igor@xbits.com.br",
            PasswordHash = "@Tester2025",
            Status = EUserStatus.Active,
            RegistrationDate = new DateTime(2024, 01, 01),
            Email = null,
            PhoneNumber = "11910734678",
            PhoneNumberConfirmed = true,
            EmailConfirmed = true,
            UserRoles =
            [
                new UserRole
                {
                    RoleId = new Guid("734b18b5-1291-4ce8-943d-1356479306da")
                }
            ],
        }
    ];
}
using System.Security.Claims;
using ArqEmCamadas.Domain.ValueObjects;

namespace ArqEmCamadas.Domain.Extensions;

public static class ClaimsPrincipalExtension
{
    public static string GetEmail(this ClaimsPrincipal user) => 
        user.FindFirst(ClaimTypes.Email)?.Value!;
    
    public static string GetName(this ClaimsPrincipal user) => 
        user.FindFirst(ClaimTypes.Name)?.Value!;
    
    public static Guid GetUserId(this ClaimsPrincipal user) => 
        Guid.Parse(user.FindFirst(ClaimTypes.NameIdentifier)?.Value!);

    public static UserCredential GetUserCredential(this ClaimsPrincipal user)
    {
        var roles = user.FindAll(ClaimTypes.Role);
        
        var id = Guid.Parse(user.FindFirst(ClaimTypes.NameIdentifier)?.Value!);


        return new UserCredential
        {
            Id = id,
            Roles = roles.Select(r => r.Value).ToList()
        };
    }

    public static UserFullData GetUserFullData(this ClaimsPrincipal user)
    {
        var roles = user.FindAll(ClaimTypes.Role);
        
        var id = Guid.Parse(user.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
        
        var name = user.FindFirst(ClaimTypes.Name)?.Value!;

        return new UserFullData
        {
            Id = id,
            Name = name,
            Roles = roles.Select(r => r.Value).ToList()
        };
    }
}
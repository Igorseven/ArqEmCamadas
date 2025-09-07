using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace ArqEmCamadas.Api.Extensions;

public static class JwtSecTokenExtension
{
    public static Guid GetUserIdFromToken(this string token)
    {
        var handler = new JwtSecurityTokenHandler();
        
        var jwtToken = handler.ReadJwtToken(token);
        
        var userIdClaim = jwtToken.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.NameIdentifier); 

        return Guid.Parse(userIdClaim!.Value);
    }
}
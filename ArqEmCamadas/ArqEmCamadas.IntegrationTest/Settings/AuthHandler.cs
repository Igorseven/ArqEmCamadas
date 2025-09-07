using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ArqEmCamadas.IntegrationTest.Settings;

public sealed class TestAuthHandler(
    IOptionsMonitor<TestAuthHandlerOptions> options,
    ILoggerFactory logger,
    UrlEncoder encoder)
    : AuthenticationHandler<TestAuthHandlerOptions>(options, logger, encoder)
{
    public const string AuthenticationScheme = "Test";

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        var identity = new ClaimsIdentity(DefaultClaims(), AuthenticationScheme);
        var principal = new ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(principal, AuthenticationScheme);

        return Task.FromResult(AuthenticateResult.Success(ticket));
    }

    private IEnumerable<Claim> DefaultClaims() =>
    [
        new Claim(ClaimTypes.Name, "user.UserName!"),
        new Claim(ClaimTypes.NameIdentifier, Options.DefaultUserId),
        new Claim(ClaimTypes.Role, Options.Role)
    ];
    
}
using Microsoft.AspNetCore.Authentication;

namespace ArqEmCamadas.IntegrationTest.Settings;

public sealed class TestAuthHandlerOptions : AuthenticationSchemeOptions
{
    public readonly string DefaultUserId = "25693911-6336-4d2b-af29-023c5babe3f3";
    public string Role { get; set; } = "Role";
}
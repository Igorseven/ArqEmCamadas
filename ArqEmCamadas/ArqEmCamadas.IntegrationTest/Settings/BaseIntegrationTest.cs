using Microsoft.Extensions.DependencyInjection;

namespace ArqEmCamadas.IntegrationTest.Settings;

public abstract class BaseIntegrationTest(
    IntegrationTestWebAppFactory factory)
    : IClassFixture<IntegrationTestWebAppFactory>
{
    protected readonly HttpClient Client = factory.CreateClient();
    protected readonly IServiceScope Scope = factory.Services.CreateScope();
}
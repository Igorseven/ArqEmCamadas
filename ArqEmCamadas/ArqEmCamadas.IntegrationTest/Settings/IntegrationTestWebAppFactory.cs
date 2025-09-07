using ArqEmCamadas.Infra.ORM.Contexts;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace ArqEmCamadas.IntegrationTest.Settings;

public abstract class IntegrationTestWebAppFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private string _role = "Role";

    private string DefaultUserId { get; set; } = "25693911-6336-4d2b-af29-023c5babe3f3";

    private const string TestConnectionString = "Data Source=(local)\\SQLEXPRESS;Initial Catalog=Partilha_test;Trusted_Connection=True;Encrypt=False;MultipleActiveResultSets=true;Pooling=True;Min Pool Size=5;Max Pool Size=20;";

    public void SetRule(string role) =>
        _role = role;

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        base.ConfigureWebHost(builder);

        builder.ConfigureTestServices(services =>
        {
            services.AddAuthentication(TestAuthHandler.AuthenticationScheme)
                .AddScheme<TestAuthHandlerOptions, TestAuthHandler>(
                    TestAuthHandler.AuthenticationScheme,
                    options => options.Role = _role);

            services.RemoveAll(typeof(DbContextOptions<ApplicationContext>));

            services.AddDbContext<ApplicationContext>(options =>
            {
                options.UseSqlServer(TestConnectionString);
            });
        });
    }

    public async Task InitializeAsync()
    {
        using var scope = Services.CreateScope();

        var scopeServices = scope.ServiceProvider;

        var context = scopeServices.GetRequiredService<ApplicationContext>();

        await context.Database.EnsureDeletedAsync();
        await context.Database.EnsureCreatedAsync();
    }

    public new async Task DisposeAsync()
    {
        using var scope = Services.CreateScope();

        var scopeServices = scope.ServiceProvider;

        var context = scopeServices.GetRequiredService<ApplicationContext>();

        await context.Database.EnsureDeletedAsync();
    }
}
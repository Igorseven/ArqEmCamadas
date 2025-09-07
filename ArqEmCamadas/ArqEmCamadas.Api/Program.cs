using ArqEmCamadas.Api.IoC;
using ArqEmCamadas.Api.Settings;
using ArqEmCamadas.Api.Settings.Handlers;

var builder = WebApplication.CreateBuilder(args);
IConfiguration configuration = builder.Configuration;

builder.Services.AddInversionOfControlHandler();
builder.Services.AddSettingsControl(configuration);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddHealthChecks();

var app = builder.Build();

app.AddWebApplication(configuration);
await app.MigrateDatabaseAsync();
app.Run();

public abstract partial class Program
{
}

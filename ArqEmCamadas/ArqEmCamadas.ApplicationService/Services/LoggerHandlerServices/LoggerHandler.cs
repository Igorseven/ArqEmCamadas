using ArqEmCamadas.Domain.Handlers.LoggerHandler;
using ArqEmCamadas.Domain.Interfaces;
using ArqEmCamadas.Infra.Interfaces.RepositoryContracts;

namespace ArqEmCamadas.ApplicationService.Services.LoggerHandlerServices;

public sealed class LoggerHandler(
    IDomainLoggerRepository domainLoggerRepository)
    : ILoggerHandler
{
    private readonly List<DomainLogger> _domainLoggers = [];

    public void CreateLogger(DomainLogger logger) => _domainLoggers.Add(logger);

    public bool HasLogger() => _domainLoggers.Count > 0;
    
    public async Task SaveInDataBase() => await domainLoggerRepository.SaveRangeAsync(_domainLoggers);
}
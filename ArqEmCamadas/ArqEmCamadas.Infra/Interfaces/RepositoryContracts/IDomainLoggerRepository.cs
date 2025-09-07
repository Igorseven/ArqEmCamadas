

using ArqEmCamadas.Domain.Handlers.LoggerHandler;

namespace ArqEmCamadas.Infra.Interfaces.RepositoryContracts;

public interface IDomainLoggerRepository : IDisposable
{
    Task SaveRangeAsync(List<DomainLogger> loggers);
}
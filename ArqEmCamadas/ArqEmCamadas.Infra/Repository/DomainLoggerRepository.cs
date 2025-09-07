using ArqEmCamadas.Domain.Handlers.LoggerHandler;
using ArqEmCamadas.Infra.Interfaces.RepositoryContracts;
using ArqEmCamadas.Infra.ORM.Contexts;
using ArqEmCamadas.Infra.Repository.Base;

namespace ArqEmCamadas.Infra.Repository;

public sealed class DomainLoggerRepository(
    ApplicationContext applicationContext) 
    : RepositoryBase<DomainLogger>(applicationContext), IDomainLoggerRepository
{
    public async Task SaveRangeAsync(List<DomainLogger> loggers)
    {
        await DbSetContext.AddRangeAsync(loggers);

        await SaveInDatabaseAsync();
    }
}
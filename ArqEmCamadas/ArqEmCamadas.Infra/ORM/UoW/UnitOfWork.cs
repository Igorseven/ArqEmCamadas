using ArqEmCamadas.Domain.Interfaces;
using ArqEmCamadas.Infra.ORM.Contexts;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace ArqEmCamadas.Infra.ORM.UoW;

public sealed class UnitOfWork(
    ApplicationContext applicationContext) 
    : IUnitOfWork
{
    private readonly DatabaseFacade _databaseFacade = applicationContext.Database;

    public void CommitTransaction()
    {
        try
        {
            _databaseFacade.CommitTransaction();
        }
        catch
        {
            RollbackTransaction();
            throw;
        }
    }

    public void RollbackTransaction() => _databaseFacade.RollbackTransaction();

    public void BeginTransaction() => _databaseFacade.BeginTransaction();
}
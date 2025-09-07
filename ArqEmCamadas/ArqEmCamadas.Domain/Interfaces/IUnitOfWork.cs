namespace ArqEmCamadas.Domain.Interfaces;

public interface IUnitOfWork
{
    void CommitTransaction();
    void RollbackTransaction();
    void BeginTransaction();
}
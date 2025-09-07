using ArqEmCamadas.Domain.Handlers.LoggerHandler;

namespace ArqEmCamadas.Domain.Interfaces;

public interface ILoggerHandler
{
    void CreateLogger(DomainLogger logger);
    bool HasLogger();
    Task SaveInDataBase();
}
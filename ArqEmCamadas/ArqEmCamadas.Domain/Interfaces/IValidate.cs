using ArqEmCamadas.Domain.Handlers.ValidationSettings;

namespace ArqEmCamadas.Domain.Interfaces;

public interface IValidate<T> where T : class
{
    Task<ValidationResponse> ValidationAsync(T entity);
    ValidationResponse Validation(T entity);
}
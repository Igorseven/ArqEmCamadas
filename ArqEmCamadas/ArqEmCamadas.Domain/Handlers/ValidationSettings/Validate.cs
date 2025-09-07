using ArqEmCamadas.Domain.Extensions;
using ArqEmCamadas.Domain.Interfaces;
using FluentValidation;
using FluentValidation.Results;

namespace ArqEmCamadas.Domain.Handlers.ValidationSettings;

public abstract class Validate<T> : AbstractValidator<T>, IValidate<T> where T : class
{
    private ValidationResult? _validationResult;

    private void CreateResult(T entity) => _validationResult ??= Validate(entity);

    private async Task CreateResultAsync(T entity) => _validationResult ??= await ValidateAsync(entity);

    private Dictionary<string, string> GetErrors() => _validationResult!.Errors.ToValidationDictionary();


    public async Task<ValidationResponse> ValidationAsync(T entity)
    {
        await CreateResultAsync(entity);

        return ValidationResponse.CreateResponse(GetErrors());
    }

    public ValidationResponse Validation(T entity)
    {
        CreateResult(entity);

        return ValidationResponse.CreateResponse(GetErrors());
    }
}
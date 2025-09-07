namespace ArqEmCamadas.Domain.Handlers.ValidationSettings;
public sealed class ValidationResponse
{
    public Dictionary<string, string> Errors { get; }
    public bool Valid => Errors.Count == 0;

    private ValidationResponse(Dictionary<string, string> errors)
    {
        Errors = errors;
    }

    public static ValidationResponse CreateResponse(Dictionary<string, string> errors)
    {
        return new ValidationResponse(errors);
    }
}

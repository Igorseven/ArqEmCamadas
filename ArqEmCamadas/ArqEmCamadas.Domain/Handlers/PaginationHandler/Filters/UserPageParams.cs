namespace ArqEmCamadas.Domain.Handlers.PaginationHandler.Filters;

public sealed class UserPageParams : PageParams
{
    public string? Name { get; init; }
    public string? Email { get; init; }
    public string? Document { get; init; }
}
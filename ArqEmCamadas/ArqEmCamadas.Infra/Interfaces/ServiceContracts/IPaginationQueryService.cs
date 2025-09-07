using ArqEmCamadas.Domain.Handlers.PaginationHandler;

namespace ArqEmCamadas.Infra.Interfaces.ServiceContracts;

public interface IPaginationQueryService<T> where T : class
{
    Task<PageList<T>> CreatePaginationAsync(IQueryable<T> source, int pageSize, int pageNumber);
    
    PageList<T> CreatePagination(List<T> source, int pageSize, int pageNumber);
}
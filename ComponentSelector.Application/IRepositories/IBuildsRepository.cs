using ComponentSelector.Application.Helpers.Pagination;
using ComponentSelector.Application.Helpers.QueryParams;
using ComponentSelector.Domain.Entities;
using ComponentSelector.Domain.Specification;

namespace ComponentSelector.Application.IRepositories;

public interface IBuildsRepository
{
    Task<PagedList<Build>> GetPagedBuildsAsync(BuildQueryParams buildQueryParams,
        BaseSpecification<Build> baseSpecification);
    Task<Build?> GetBuildByIdAsync(int id, BaseSpecification<Build>? spec);
    Task<bool> CreateBuildAsync(Build build);
    Task<bool> DeleteBuildAsync(Build build);
    Task<int> GetTotalBuildsAsync();
    Task<bool> SaveChangesAsync();

}

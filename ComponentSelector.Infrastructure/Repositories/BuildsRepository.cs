using ComponentSelector.Application.Helpers.Pagination;
using ComponentSelector.Application.Helpers.QueryParams;
using ComponentSelector.Application.IRepositories;
using ComponentSelector.Application.Specification;
using ComponentSelector.Domain.Entities;
using ComponentSelector.Domain.Specification;
using ComponentSelector.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ComponentSelector.Infrastructure.Repositories;

public class BuildsRepository(AppDbContext context) : IBuildsRepository
{
    public async Task<bool> CreateBuildAsync(Build build)
    {
        await context.AddAsync(build);

        return await SaveChangesAsync();
    }

    public async Task<bool> DeleteBuildAsync(Build build)
    {
        context.Remove(build);

        return await SaveChangesAsync();
    }
    public async Task<int> GetTotalBuildsAsync()
    {
        return await context.Builds.CountAsync();
    }
    public async Task<Build?> GetBuildByIdAsync(int id, BaseSpecification<Build>? spec)
    {
        var query = context.Builds.AsNoTracking();

        if (spec != null)
            query = SpecificationQueryBuilder.GetQuery(query, spec);

        return await query.FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<PagedList<Build>> GetPagedBuildsAsync(BuildQueryParams buildQueryParams,
        BaseSpecification<Build> baseSpecification)
    {
        var buildsQuery = context.Builds.AsNoTracking();

        var specifiedBuilds = SpecificationQueryBuilder.GetQuery(buildsQuery, baseSpecification);

        return await PagedList<Build>.PaginateAsync(specifiedBuilds,
            buildQueryParams.PageNumber, buildQueryParams.PageSize);
    }

    public async Task<bool> SaveChangesAsync()
    {
        return await context.SaveChangesAsync() > 0;
    }
}

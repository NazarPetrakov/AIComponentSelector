
using ComponentSelector.Application.Helpers.Pagination;
using ComponentSelector.Application.Helpers.QueryParams;
using ComponentSelector.Application.IRepositories;
using ComponentSelector.Application.Specification;
using ComponentSelector.Domain.Entities;
using ComponentSelector.Domain.Specification;
using ComponentSelector.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ComponentSelector.Infrastructure.Repositories;

public class CharacteristicsRepository(AppDbContext context) : ICharacteristicsRepository
{
    public async Task<PagedList<Characteristic>> GetCharacteristicsAsync(
        CharacteristicsQueryParams characteristicsQueryParams,
        BaseSpecification<Characteristic> spec)
    {
        var query = context.Characteristics.AsNoTracking();

        query = SpecificationQueryBuilder.GetQuery(query, spec);

        return await PagedList<Characteristic>.PaginateAsync(query,
            characteristicsQueryParams.PageNumber, characteristicsQueryParams.PageSize);
    }
}

using ComponentSelector.Application.Helpers.Pagination;
using ComponentSelector.Application.Helpers.QueryParams;
using ComponentSelector.Domain.Entities;
using ComponentSelector.Domain.Specification;

namespace ComponentSelector.Application.IRepositories;

public interface ICharacteristicsRepository
{
    Task<PagedList<Characteristic>> GetCharacteristicsAsync(
        CharacteristicsQueryParams characteristicsQueryParams,
        BaseSpecification<Characteristic> baseSpecification);
}

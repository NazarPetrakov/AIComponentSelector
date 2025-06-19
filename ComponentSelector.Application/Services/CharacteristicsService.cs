using AutoMapper;
using ComponentSelector.Application.Contracts;
using ComponentSelector.Application.Helpers.QueryParams;
using ComponentSelector.Application.IRepositories;
using ComponentSelector.Application.IServices;
using ComponentSelector.Domain.Specification;

namespace ComponentSelector.Application.Services;

public class CharacteristicsService(ICharacteristicsRepository characteristicsRepository, IMapper mapper)
    : ICharacteristicsService
{
    public async Task<List<CharacteristicDto>> GetCharacteristicsByComponentIdAsync(int componentId,
        CharacteristicsQueryParams queryParams)
    {
        var specification = new CharacteristicSpecification(c => c.ComponentId == componentId);

        var characteristics = await characteristicsRepository
            .GetCharacteristicsAsync(queryParams, specification);

        return mapper.Map<List<CharacteristicDto>>(characteristics);
    }
}

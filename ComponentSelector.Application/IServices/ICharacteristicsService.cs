using ComponentSelector.Application.Contracts;
using ComponentSelector.Application.Helpers.QueryParams;

namespace ComponentSelector.Application.IServices;

public interface ICharacteristicsService
{
    Task<List<CharacteristicDto>> GetCharacteristicsByComponentIdAsync(int componentId,
        CharacteristicsQueryParams queryParams);
}

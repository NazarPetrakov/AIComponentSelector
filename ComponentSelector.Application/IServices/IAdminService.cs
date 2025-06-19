using ComponentSelector.Application.Contracts;

namespace ComponentSelector.Application.IServices;

public interface IAdminService
{
    Task<StatisticsDto> GetStatisticsAsync();
}

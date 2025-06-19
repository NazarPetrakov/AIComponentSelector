using ComponentSelector.Application.Contracts;
using ComponentSelector.Application.IRepositories;
using ComponentSelector.Application.IServices;

namespace ComponentSelector.Application.Services;

public class AdminService(IUsersRepository usersRepository,
    IComponentsRepository componentsRepository, IBuildsRepository buildsRepository) : IAdminService
{
    public async Task<StatisticsDto> GetStatisticsAsync()
    {
        return new StatisticsDto
        {
            TotalUsers = await usersRepository.GetTotalUsersAsync(),
            TotalComponents = await componentsRepository.GetTotalComponentsAsync(),
            TotalBuilds = await buildsRepository.GetTotalBuildsAsync(),
        };
    }
}

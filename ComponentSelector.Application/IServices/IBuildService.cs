using ComponentSelector.Application.Contracts;
using ComponentSelector.Application.Contracts.Build;

namespace ComponentSelector.Application.IServices;

public interface IBuildService
{
    Task<BuildDto> BuildPC(CreateBuildDto createBuildDto);
    Task<BuildDto> AIBuildPC(ChatBuildRequest request);
}

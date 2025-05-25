using AutoMapper;
using ComponentSelector.Application.Contracts;
using ComponentSelector.Application.Contracts.Build;
using ComponentSelector.Application.Extensions;
using ComponentSelector.Application.Helpers.Pagination;
using ComponentSelector.Application.Helpers.QueryParams;
using ComponentSelector.Application.IRepositories;
using ComponentSelector.Application.IServices;
using ComponentSelector.Domain.Entities;
using ComponentSelector.Domain.Enums;
using ComponentSelector.Domain.Exceptions;
using ComponentSelector.Domain.Specification;
using Microsoft.AspNetCore.Http;

namespace ComponentSelector.Application.Services;

public class BuildService(IAppOpenAIService openAIService,
    IComponentsService componentsService, IBuildsRepository buildsRepository,
    IMapper mapper) : IBuildService
{
    public async Task<BuildDto> AIBuildPC(ChatBuildRequest request)
    {
        ChatBuildResponse chatBuild = await openAIService.GetChatBuildAsync(request);

        return await CreateAiBuildAsync(chatBuild);

        // return await CreateBuildAsync(new Dictionary<CategoryEnum, string?>
        // {
        //     { CategoryEnum.Processors, chatBuild.Build?.CPU.Title },
        //     { CategoryEnum.Motherboards, chatBuild.Build?.Motherboard.Title },
        //     { CategoryEnum.Memory, chatBuild.Build?.RAM.Title },
        //     { CategoryEnum.SSD, chatBuild.Build?.Storage.Title },
        //     { CategoryEnum.Videocards, chatBuild.Build?.GPU.Title },
        //     { CategoryEnum.PSU, chatBuild.Build?.PSU.Title },
        //     { CategoryEnum.Cases, chatBuild.Build?.Case.Title }
        // }, chatBuild.Compatibility);
    }

    public async Task<BuildDto> BuildPC(SearchBuildDto dto)
    {
        return await CreateBuildAsync(new Dictionary<CategoryEnum, string?>
        {
            { CategoryEnum.Processors, dto.CPUTitle },
            { CategoryEnum.Motherboards, dto.MotherboardTitle },
            { CategoryEnum.Memory, dto.RAMTitle },
            { CategoryEnum.SSD, dto.StorageTitle },
            { CategoryEnum.Videocards, dto.GPUTitle },
            { CategoryEnum.PSU, dto.PSUTitle },
            { CategoryEnum.Cases, dto.CaseTitle }
        });
    }

    public async Task<int> CreateBuildAsync(CreateUserBuildDto createBuild, string userId)
    {
        Build build = mapper.Map<Build>(createBuild);

        build.TotalPrice += await ValidateAndGetComponentPriceAsync(createBuild.CPUId, CategoryEnum.Processors,
            nameof(createBuild.CPUId)) ?? 0;
        build.TotalPrice += await ValidateAndGetComponentPriceAsync(createBuild.MotherboardId, CategoryEnum.Motherboards,
            nameof(createBuild.MotherboardId)) ?? 0;
        build.TotalPrice += await ValidateAndGetComponentPriceAsync(createBuild.RAMId, CategoryEnum.Memory,
            nameof(createBuild.RAMId)) ?? 0;
        build.TotalPrice += await ValidateAndGetComponentPriceAsync(createBuild.StorageId, CategoryEnum.SSD,
            nameof(createBuild.StorageId)) ?? 0;
        build.TotalPrice += await ValidateAndGetComponentPriceAsync(createBuild.GPUId, CategoryEnum.Videocards,
            nameof(createBuild.GPUId)) ?? 0;
        build.TotalPrice += await ValidateAndGetComponentPriceAsync(createBuild.PSUId, CategoryEnum.PSU,
            nameof(createBuild.PSUId)) ?? 0;
        build.TotalPrice += await ValidateAndGetComponentPriceAsync(createBuild.CaseId, CategoryEnum.Cases,
            nameof(createBuild.CaseId)) ?? 0;

        build.UserId = int.Parse(userId);

        bool successfulCreating = await buildsRepository.CreateBuildAsync(build);

        if (!successfulCreating) throw new SaveChangesException("Failed to save the build to the database.");

        return build.Id;
    }
    public async Task DeleteBuildAsync(int buildId, string userId)
    {
        var build = await buildsRepository.GetBuildByIdAsync(buildId, null) ??
            throw new ItemNotFoundException($"Build with id: {buildId} not found.");

        if (build.UserId != int.Parse(userId))
        {
            throw new ForbiddenAccessException("You are not authorized to delete this build.");
        }

        bool successfulDeleting = await buildsRepository.DeleteBuildAsync(build);

        if (!successfulDeleting) throw new SaveChangesException("Failed to delete the build from the database.");
    }

    public async Task<UserBuildDto> GetBuildByIdAsync(int id)
    {
        BaseSpecification<Build> spec = new BuildWithComponentsSpecification();

        Build build = await buildsRepository.GetBuildByIdAsync(id, spec) ??
            throw new ItemNotFoundException($"Build with id: {id} not found.");

        return mapper.Map<UserBuildDto>(build);
    }

    public async Task<PagedList<UserBuildDto>> GetPagedBuildsAsync(BuildQueryParams buildQueryParams,
        HttpResponse httpResponse, BaseSpecification<Build> spec)
    {
        var builds = await buildsRepository.GetPagedBuildsAsync(buildQueryParams,
           spec);

        httpResponse.AddPaginationHeader(builds);

        var buildsDtos = mapper.Map<List<UserBuildDto>>(builds);

        return new PagedList<UserBuildDto>
        (
            buildsDtos,
            builds.TotalCount,
            builds.CurrentPage,
            builds.PageSize
        );
    }

    public async Task<PagedList<UserBuildDto>> GetPagedUserBuildsAsync(string userId,
        BuildQueryParams buildQueryParams, HttpResponse httpResponse)
    {
        var id = int.Parse(userId);

        return await GetPagedBuildsAsync(buildQueryParams, httpResponse,
                new BuildWithComponentsSpecification(b => b.UserId == id));
    }

    private async Task<double?> ValidateAndGetComponentPriceAsync(int? componentId, CategoryEnum expectedCategory,
        string fieldName)
    {
        if (!componentId.HasValue)
            return 0;

        var component = await componentsService.GetComponentByIdAsync(componentId.Value);

        if (!string.Equals(component.Category, expectedCategory.ToString(), StringComparison.OrdinalIgnoreCase))
            throw new InvalidOperationException(
                $"{fieldName} with Id: {componentId.Value} is not a valid {expectedCategory} component.");

        return component.Price;
    }
    private async Task<BuildDto> CreateAiBuildAsync(ChatBuildResponse chatBuildResponse)
    {
        static string EnsureNotNull(string? value, CategoryEnum category)
            => value ?? throw new ArgumentNullException(nameof(value), $"Component title for {category} cannot be null.");

        var cpu = await componentsService.FindComponentByTitleAsync(
            CategoryEnum.Processors,
            EnsureNotNull(chatBuildResponse.Build?.CPU.Title, CategoryEnum.Processors));
        var motherboard = await componentsService.FindComponentByTitleAsync(
            CategoryEnum.Motherboards,
            EnsureNotNull(chatBuildResponse.Build?.Motherboard.Title, CategoryEnum.Motherboards));
        var ram = await componentsService.FindComponentByTitleAsync(
            CategoryEnum.Memory,
            EnsureNotNull(chatBuildResponse.Build?.RAM.Title, CategoryEnum.Memory));
        var storage = await componentsService.FindComponentByTitleAsync(
            CategoryEnum.SSD,
            EnsureNotNull(chatBuildResponse.Build?.Storage.Title, CategoryEnum.SSD));
        var gpu = await componentsService.FindComponentByCharacteristicsAsync(
            CategoryEnum.Videocards, chatBuildResponse.Build!.GPU.Characteristics);
        var psu = await componentsService.FindComponentByTitleAsync(
            CategoryEnum.PSU,
            EnsureNotNull(chatBuildResponse.Build?.PSU.Title, CategoryEnum.PSU));
        var pcCase = await componentsService.FindComponentByTitleAsync(
            CategoryEnum.Cases,
            EnsureNotNull(chatBuildResponse.Build?.Case.Title, CategoryEnum.Cases));

        // var all = new List<ComponentDto> { cpu, motherboard, ram, storage, gpu, psu, pcCase };

        return new BuildDto
        {
            CPU = cpu,
            Motherboard = motherboard,
            RAM = ram,
            Storage = storage,
            GPU = gpu,
            PSU = psu,
            Case = pcCase,
            TotalPrice = chatBuildResponse.TotalPrice,
            Compatibility = chatBuildResponse.Compatibility,
            ChatBuildResponse = chatBuildResponse
        };
    }
    private async Task<BuildDto> CreateBuildAsync(Dictionary<CategoryEnum, string?> components)
    {
        static string EnsureNotNull(string? value, CategoryEnum category)
            => value ?? throw new ArgumentNullException(nameof(value), $"Component title for {category} cannot be null.");

        var cpu = await componentsService.FindComponentByTitleAsync(
            CategoryEnum.Processors,
            EnsureNotNull(components[CategoryEnum.Processors], CategoryEnum.Processors));
        var motherboard = await componentsService.FindComponentByTitleAsync(
            CategoryEnum.Motherboards,
            EnsureNotNull(components[CategoryEnum.Motherboards], CategoryEnum.Motherboards));
        var ram = await componentsService.FindComponentByTitleAsync(
            CategoryEnum.Memory,
            EnsureNotNull(components[CategoryEnum.Memory], CategoryEnum.Memory));
        var storage = await componentsService.FindComponentByTitleAsync(
            CategoryEnum.SSD,
            EnsureNotNull(components[CategoryEnum.SSD], CategoryEnum.SSD));
        var gpu = await componentsService.FindComponentByTitleAsync(
            CategoryEnum.Videocards,
            EnsureNotNull(components[CategoryEnum.Videocards], CategoryEnum.Videocards));
        var psu = await componentsService.FindComponentByTitleAsync(
            CategoryEnum.PSU,
            EnsureNotNull(components[CategoryEnum.PSU], CategoryEnum.PSU));
        var pcCase = await componentsService.FindComponentByTitleAsync(
            CategoryEnum.Cases,
            EnsureNotNull(components[CategoryEnum.Cases], CategoryEnum.Cases));

        var all = new List<ComponentDto> { cpu, motherboard, ram, storage, gpu, psu, pcCase };

        return new BuildDto
        {
            CPU = cpu,
            Motherboard = motherboard,
            RAM = ram,
            Storage = storage,
            GPU = gpu,
            PSU = psu,
            Case = pcCase,
            TotalPrice = all.Sum(x => x.Price ?? 0),
        };
    }


}

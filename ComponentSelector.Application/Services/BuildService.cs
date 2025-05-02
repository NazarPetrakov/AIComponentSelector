using ComponentSelector.Application.Contracts;
using ComponentSelector.Application.Contracts.Build;
using ComponentSelector.Application.IServices;
using ComponentSelector.Domain.Enums;

namespace ComponentSelector.Application.Services;

public class BuildService(IOpenAIService openAIService, IComponentsService componentsService) : IBuildService
{
    public async Task<BuildDto> AIBuildPC(ChatBuildRequest request)
    {
        ChatBuildResponse chatBuild = await openAIService.GetChatBuildAsync(request);

        return await CreateBuildAsync(new Dictionary<CategoryEnum, string?>
        {
            { CategoryEnum.Processors, chatBuild.Build?.CPU.Title },
            { CategoryEnum.Motherboards, chatBuild.Build?.Motherboard.Title },
            { CategoryEnum.Memory, chatBuild.Build?.RAM.Title },
            { CategoryEnum.SSD, chatBuild.Build?.Storage.Title },
            { CategoryEnum.Videocards, chatBuild.Build?.GPU.Title },
            { CategoryEnum.PSU, chatBuild.Build?.PSU.Title },
            { CategoryEnum.Cases, chatBuild.Build?.Case.Title }
        }, chatBuild.Compatibility);
    }

    public async Task<BuildDto> BuildPC(CreateBuildDto dto)
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
    private async Task<BuildDto> CreateBuildAsync(Dictionary<CategoryEnum, string?> components, Compatibility? compatibility = null)
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
            Compatibility = compatibility
        };
    }


}

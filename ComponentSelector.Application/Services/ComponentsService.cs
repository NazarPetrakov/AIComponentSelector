using AutoMapper;
using ComponentSelector.Application.Contracts;
using ComponentSelector.Application.Extensions;
using ComponentSelector.Application.Helpers.Pagination;
using ComponentSelector.Application.Helpers.QueryParams;
using ComponentSelector.Application.IRepositories;
using ComponentSelector.Application.IServices;
using ComponentSelector.Application.Specification;
using ComponentSelector.Domain.Entities;
using ComponentSelector.Domain.Enums;
using ComponentSelector.Domain.Exceptions;
using ComponentSelector.Domain.Specification;
using FuzzySharp;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace ComponentSelector.Application.Services;

public class ComponentsService(IComponentsRepository componentsRepository,
    IMapper mapper) : IComponentsService
{
    public async Task<ComponentWithCharacteristicsDto> GetComponentByIdAsync(int id)
    {
        var specification = new ComponentWithCharacteristicsSpecification();
        
        var component = await componentsRepository.GetComponentByIdAsync(id, specification) ??
            throw new ItemNotFoundException($"Component with id: {id} not found.");

        return mapper.Map<ComponentWithCharacteristicsDto>(component);
    }
    public async Task<PagedList<ComponentDto>> GetComponentsAsync(
        HttpResponse httpResponse, ComponentQueryParams componentQueryParams)
    {
        var specification = new FilteredComponentSpecification(componentQueryParams);
        var components = await componentsRepository.GetPagedComponentsAsync(componentQueryParams,
           specification);

        httpResponse.AddPaginationHeader(components);

        var componentDtos = mapper.Map<List<ComponentDto>>(components);

        return new PagedList<ComponentDto>
        (
            componentDtos,
            components.TotalCount,
            components.CurrentPage,
            components.PageSize
        );
    }
    public async Task<List<ComponentWithScoreDto>> FindTopComponentsWithScoreAsync
        (CategoryEnum category, string searchName, int limit)
    {
        var specification = new ComponentSpecification(c => c.Category == category.ToString());

        var titles = await componentsRepository.GetComponentTitlesAsync(specification);

        var matches = Process.ExtractTop(
            searchName.ToLowerInvariant(),
            titles.Select(t => t.ToLowerInvariant()).ToList(),
            limit: limit)
            .Where(m => m.Score > 50)
            .ToList();

        if (!matches.Any())
            return new List<ComponentWithScoreDto>();

        var components = await componentsRepository.GetSimpleComponentsAsync(specification);

        var results = matches.Select(m =>
        {
            var component = components.First(c =>
            (c.Title?.ToLowerInvariant() ?? "") == m.Value);
            return new ComponentWithScoreDto
            {
                SimpleComponent = component,
                Score = m.Score
            };
        }).ToList();

        return results;
    }

    public async Task<ComponentDto> FindComponentByTitleAsync(CategoryEnum category, string title)
    {
        var scoredComponents = await FindTopComponentsWithScoreAsync(category, title, 10);
        var bestMatch = scoredComponents.MaxBy(x => x.Score);

        if (bestMatch != null)
        {
            var result = await componentsRepository.GetComponentByIdAsync(bestMatch.SimpleComponent.Id, null)
                ?? throw new ItemNotFoundException($"Component with id: {bestMatch.SimpleComponent.Id} not found.");
            return mapper.Map<ComponentDto>(result);
        }

        return new ComponentDto();
    }
    public async Task<ComponentDto?> FindComponentByCharacteristicsAsync(CategoryEnum category,
        List<CharacteristicDto> chatCharacteristics)
    {
        BaseSpecification<Component> spec = new ComponentWithCharacteristicsSpecification();
        if (category == CategoryEnum.Videocards)
        {
            string chatGpuBrand = chatCharacteristics.FirstOrDefault(c => c.AttributeName == "brand")
                ?.AttributeValue.ToLower() ?? "";
            string chatGpuChip = chatCharacteristics.FirstOrDefault(c => c.AttributeName == "chip")
                ?.AttributeValue.ToLower() ?? "";
            spec = new ComponentWithCharacteristicsSpecification(c =>
                c.Characteristics.Any(ch => ch.AttributeName.ToLower() == "графічний чип"
                    && ch.AttributeValue.ToLower() == chatGpuChip) && (c.Title ?? "").Contains(chatGpuBrand));
        }
        var component = await componentsRepository.GetComponentsQuery(spec).FirstOrDefaultAsync();

        return mapper.Map<ComponentDto>(component);

        // var scoredComponents = await FindTopComponentsWithScoreAsync(category, title, 10);
        // var bestMatch = scoredComponents.MaxBy(x => x.Score);

        // if (bestMatch != null)
        // {
        //     var result = await componentsRepository.GetComponentByIdAsync(bestMatch.SimpleComponent.Id, null)
        //         ?? throw new ItemNotFoundException($"Component with id: {bestMatch.SimpleComponent.Id} not found.");
        //     return mapper.Map<ComponentDto>(result);
        // }

        // return new ComponentDto();
    }
}

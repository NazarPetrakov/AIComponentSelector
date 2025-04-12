using System.Linq.Expressions;
using ComponentSelector.Application.Helpers.QueryParams;
using ComponentSelector.Domain.Entities;
using ComponentSelector.Domain.Specification;
using Microsoft.EntityFrameworkCore;

namespace ComponentSelector.Application.Specification;

public class FilteredComponentSpecification : ComponentSpecification
{
    private static readonly string[] _availableStatuses =
    {
        "Є в наявності",
        "Закінчується",
        "Готовий до відправлення",
        "Передзамовлення"
    };
    public FilteredComponentSpecification(ComponentQueryParams componentParams)
        : base(BuildCriteria(componentParams))
    {
        if (!string.IsNullOrWhiteSpace(componentParams.SearchTerm))
        {
            var searchOrderByExpression = GetSearchOrderByExpression(componentParams.SearchTerm.ToLower());
            AddOrderBy(searchOrderByExpression);
        }
        var orderByExpression = GetOrderByExpression(componentParams.OrderBy);
        var orderByDescExpression = GetOrderByExpression(componentParams.OrderByDesc);

        if (orderByDescExpression != null)
            AddOrderByDesc(orderByDescExpression);
        else if (orderByExpression != null)
            AddOrderBy(orderByExpression);
    }

    private static Expression<Func<Component, bool>> BuildCriteria(ComponentQueryParams componentParams)
    {
        Expression<Func<Component, bool>> criteria = c => true;

        if (!string.IsNullOrWhiteSpace(componentParams.Category))
            criteria = criteria.And(c => (c.Category ?? "").ToLower() == componentParams.Category);

        if (componentParams.MinPrice.HasValue)
            criteria = criteria.And(c => c.Price >= componentParams.MinPrice.Value);

        if (componentParams.MaxPrice.HasValue)
            criteria = criteria.And(c => c.Price <= componentParams.MaxPrice.Value);

        if (componentParams.Availability.HasValue)
            if (componentParams.Availability.Value)
                criteria = criteria.And(c => _availableStatuses.Contains(c.Availability));
            else
                criteria = criteria.And(c => !_availableStatuses.Contains(c.Availability));

        if (!string.IsNullOrWhiteSpace(componentParams.SearchTerm))
        {
            var terms = componentParams.SearchTerm
                .ToLower()
                .Split(' ', StringSplitOptions.RemoveEmptyEntries);

            foreach (var term in terms)
            {
                criteria = criteria.And(c => (c.Title ?? "").ToLower().Contains(term));
            }
        }

        return criteria;
    }
    private static Expression<Func<Component, object>>? GetOrderByExpression(string? orderBy)
    {
        return orderBy?.ToLower() switch
        {
            "id" => c => c.Id,
            "category" => c => c.Category!,
            "price" => c => c.Price!,
            "availability" => c => c.Availability!,
            "title" => c => c.Title!,
            "reviews" => c => c.Reviews!,
            _ => null
        };
    }
    public static Expression<Func<Component, object>> GetSearchOrderByExpression(string searchTerm)
    {
        return c =>
        (c.Title != null && EF.Functions.Like(c.Title, searchTerm + "%")) ? 0 :
        (c.Title != null && EF.Functions.Like(c.Title, "% " + searchTerm + " %")) ? 1 :
        2;
    }
}

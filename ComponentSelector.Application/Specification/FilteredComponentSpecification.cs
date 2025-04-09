using System.Linq.Expressions;
using ComponentSelector.Application.Helpers.QueryParams;
using ComponentSelector.Domain.Entities;
using ComponentSelector.Domain.Specification;

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

        return criteria;
    }
    private static Expression<Func<Component, object>>? GetOrderByExpression(string? orderBy)
    {
        return orderBy?.ToLower() switch
        {
            "id" => c => c.Id,
            "category" => c => c.Category!,
            "price" => c => c.Price!,
            // "availability" => c => c.Availability!,
            // "title" => c => c.Title!,
            _ => null
        };
    }
}

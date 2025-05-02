using System.Linq.Expressions;
using ComponentSelector.Domain.Entities;

namespace ComponentSelector.Domain.Specification;

public class ComponentSpecification : BaseSpecification<Component>
{
    public ComponentSpecification()
    {
    }

    public ComponentSpecification(Expression<Func<Component, bool>> criteria)
        : base(criteria)
    {
    }
}
public class ComponentWithCharacteristicsSpecification : BaseSpecification<Component>
{
    public ComponentWithCharacteristicsSpecification()
    {
        AddInclude(c => c.Characteristics);
    }
}
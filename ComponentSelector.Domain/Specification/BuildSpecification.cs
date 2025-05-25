using System.Linq.Expressions;
using ComponentSelector.Domain.Entities;

namespace ComponentSelector.Domain.Specification;

public class BuildSpecification : BaseSpecification<Build>
{
    public BuildSpecification()
    {
    }

    public BuildSpecification(Expression<Func<Build, bool>> criteria)
        : base(criteria)
    {
    }
}
public class BuildWithComponentsSpecification : BaseSpecification<Build>
{
    public BuildWithComponentsSpecification()
    {
        AddInclude(b => b.CPU!);
        AddInclude(b => b.Motherboard!);
        AddInclude(b => b.RAM!);
        AddInclude(b => b.Storage!);
        AddInclude(b => b.GPU!);
        AddInclude(b => b.PSU!);
        AddInclude(b => b.Case!);
        AddOrderByDesc(b => b.Id);


    }
    public BuildWithComponentsSpecification(Expression<Func<Build, bool>> criteria)
        : base(criteria)
    {
        AddInclude(b => b.CPU!);
        AddInclude(b => b.Motherboard!);
        AddInclude(b => b.RAM!);
        AddInclude(b => b.Storage!);
        AddInclude(b => b.GPU!);
        AddInclude(b => b.PSU!);
        AddInclude(b => b.Case!);
        AddOrderByDesc(b => b.Id);
    }
}

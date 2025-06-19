using System.Linq.Expressions;
using ComponentSelector.Domain.Entities;

namespace ComponentSelector.Domain.Specification;

public class CharacteristicSpecification : BaseSpecification<Characteristic>
{
    public CharacteristicSpecification()
    {
    }

    public CharacteristicSpecification(Expression<Func<Characteristic, bool>> criteria)
        : base(criteria)
    {
    }
}

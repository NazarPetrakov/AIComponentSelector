using Microsoft.AspNetCore.Identity;

namespace ComponentSelector.Domain.Exceptions;

public class IdentityException : Exception
{
    public IdentityException() : base()
    {
    }
    public IdentityException(string message) : base(message)
    {
    }
    public IdentityException(IdentityResult result)
        : base(string.Join("; ", result.Errors.Select(e => e.Description)))
    {
    }
}

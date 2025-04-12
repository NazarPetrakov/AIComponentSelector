using System.Security.Claims;

namespace ComponentSelector.API.Extensions;

public static class ClaimsPrincipalExtension
{
    public static string GetUserId(this ClaimsPrincipal user)
    {
        var id = user.FindFirstValue(ClaimTypes.NameIdentifier
            ?? throw new InvalidOperationException("Cannot get id from token"));
        return id!;
    }
}

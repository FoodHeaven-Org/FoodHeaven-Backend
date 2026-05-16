using System.Security.Claims;

namespace food_heaven_backend.Shared.Interfaces.Rest;

public static class ClaimsPrincipalExtensions
{
    public static int GetAuthenticatedUserId(this ClaimsPrincipal user)
    {
        var userId = user.FindFirstValue(ClaimTypes.Sid);

        if (!int.TryParse(userId, out var parsedUserId) || parsedUserId <= 0)
        {
            throw new UnauthorizedAccessException("The authenticated user id is missing or invalid.");
        }

        return parsedUserId;
    }
}

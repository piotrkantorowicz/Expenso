using Expenso.Api.Configuration.Auth.Claims;
using Expenso.Shared.UserContext;

namespace Expenso.Api.Configuration.Auth.Users.UserContext;

internal sealed class UserContextAccessor(IHttpContextAccessor httpContextAccessor) : IUserContextAccessor
{
    private readonly IHttpContextAccessor _httpContextAccessor =
        httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));

    public IUserContext? Get()
    {
        if (_httpContextAccessor.HttpContext?.User.Identity?.IsAuthenticated == false)
        {
            return null;
        }

        string? userId = GetClaim(ClaimNames.UserIdClaimName);
        string? username = GetClaim(ClaimNames.UsernameClaimName);
        UserContext userContext = new(userId, username);

        return userContext;
    }

    private string? GetClaim(string claimName)
    {
        return _httpContextAccessor.HttpContext?.User.Claims.SingleOrDefault(x => x.Type == claimName)?.Value;
    }
}
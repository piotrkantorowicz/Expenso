using System.Security.Claims;

namespace Expenso.Api.Configuration.Auth.Claims;

internal static class ClaimNames
{
    public const string UserIdClaimName = ClaimTypes.NameIdentifier;
    public const string UsernameClaimName = "name";
}
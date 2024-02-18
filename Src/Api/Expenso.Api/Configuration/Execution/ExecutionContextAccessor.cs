using Expenso.Api.Configuration.Auth.Claims;
using Expenso.Api.Configuration.Auth.Users;
using Expenso.Api.Configuration.Execution.Middlewares;
using Expenso.Shared.System.Types.ExecutionContext;
using Expenso.Shared.System.Types.ExecutionContext.Models;

using Microsoft.Extensions.Primitives;

namespace Expenso.Api.Configuration.Execution;

internal sealed class ExecutionContextAccessor(IHttpContextAccessor httpContextAccessor) : IExecutionContextAccessor
{
    private readonly IHttpContextAccessor _httpContextAccessor =
        httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));

    public IExecutionContext Get()
    {
        Guid? correlationId = null;

        if (_httpContextAccessor.HttpContext?.Request.Headers.Keys.Any(x =>
                x == CorrelationIdMiddleware.CorrelationHeaderKey) == true)
        {
            StringValues correlationIdString =
                _httpContextAccessor.HttpContext.Request.Headers[CorrelationIdMiddleware.CorrelationHeaderKey];

            if (Guid.TryParse(correlationIdString, out Guid correlationIdValue))
            {
                correlationId = correlationIdValue;
            }
        }

        UserContext? userContext = null;

        if (_httpContextAccessor.HttpContext?.User.Identity?.IsAuthenticated == true)
        {
            string? userId = GetClaim(ClaimNames.UserIdClaimName);
            string? username = GetClaim(ClaimNames.UsernameClaimName);
            userContext = new UserContext(userId, username);
        }

        return new ExecutionContext(correlationId, userContext);
    }

    private string? GetClaim(string claimName)
    {
        return _httpContextAccessor.HttpContext?.User.Claims.SingleOrDefault(x => x.Type == claimName)?.Value;
    }
}
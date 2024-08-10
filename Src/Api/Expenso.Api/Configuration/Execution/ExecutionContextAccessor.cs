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
        httpContextAccessor ?? throw new ArgumentNullException(paramName: nameof(httpContextAccessor));

    public IExecutionContext Get()
    {
        Guid? correlationId = null;

        if (_httpContextAccessor.HttpContext?.Request.Headers.Keys.Any(predicate: x =>
                x == CorrelationIdMiddleware.CorrelationHeaderKey) == true)
        {
            StringValues correlationIdString =
                _httpContextAccessor.HttpContext.Request.Headers[key: CorrelationIdMiddleware.CorrelationHeaderKey];

            if (Guid.TryParse(input: correlationIdString, result: out Guid correlationIdValue))
            {
                correlationId = correlationIdValue;
            }
        }

        UserContext? userContext = null;

        if (_httpContextAccessor.HttpContext?.User.Identity?.IsAuthenticated == true)
        {
            string? userId = GetClaim(claimName: ClaimNames.UserIdClaimName);
            string? username = GetClaim(claimName: ClaimNames.UsernameClaimName);
            userContext = new UserContext(UserId: userId, Username: username);
        }

        return new ExecutionContext(CorrelationId: correlationId, UserContext: userContext);
    }

    private string? GetClaim(string claimName)
    {
        return _httpContextAccessor.HttpContext?.User.Claims.SingleOrDefault(predicate: x => x.Type == claimName)
            ?.Value;
    }
}
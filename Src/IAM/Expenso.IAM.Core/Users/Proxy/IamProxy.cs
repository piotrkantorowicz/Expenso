using Expenso.IAM.Core.Users.Internal.Queries.GetUser;
using Expenso.IAM.Proxy;
using Expenso.IAM.Proxy.DTO.GetUser;
using Expenso.Shared.Queries.Dispatchers;

namespace Expenso.IAM.Core.Users.Proxy;

internal sealed class IamProxy(IQueryDispatcher queryDispatcher) : IIamProxy
{
    private readonly IQueryDispatcher _queryDispatcher =
        queryDispatcher ?? throw new ArgumentNullException(nameof(queryDispatcher));

    public async Task<GetUserInternalResponse?> GetUserByIdAsync(string userId,
        CancellationToken cancellationToken = default)
    {
        return await _queryDispatcher.QueryAsync(new GetUserQuery(userId), cancellationToken);
    }

    public async Task<GetUserInternalResponse?> GetUserByEmailAsync(string email,
        CancellationToken cancellationToken = default)
    {
        return await _queryDispatcher.QueryAsync(new GetUserQuery(Email: email), cancellationToken);
    }
}

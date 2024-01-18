using Expenso.IAM.Core.Users.Queries.GetUserInternal;
using Expenso.IAM.Proxy;
using Expenso.IAM.Proxy.DTO.GetUser;
using Expenso.Shared.Queries;

namespace Expenso.IAM.Core.Users.Proxy;

internal sealed class IamUsersUsersProxy(IQueryDispatcher queryDispatcher) : IIamUsersProxy
{
    private readonly IQueryDispatcher _queryDispatcher =
        queryDispatcher ?? throw new ArgumentNullException(nameof(queryDispatcher));

    public async Task<GetUserInternalResponse?> GetUserByIdAsync(string userId)
    {
        return await _queryDispatcher.QueryAsync(new GetUserInternalQuery(userId));
    }

    public async Task<GetUserInternalResponse?> GetUserByEmailAsync(string email)
    {
        return await _queryDispatcher.QueryAsync(new GetUserInternalQuery(Email: email));
    }
}
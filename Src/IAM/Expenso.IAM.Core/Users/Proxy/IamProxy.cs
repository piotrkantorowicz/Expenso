using Expenso.IAM.Core.Users.Queries.GetUser;
using Expenso.IAM.Core.Users.Queries.GetUser.DTO.Response;
using Expenso.IAM.Core.Users.Queries.GetUser.DTO.Response.Maps;
using Expenso.IAM.Proxy;
using Expenso.IAM.Proxy.DTO.GetUser;
using Expenso.Shared.Queries.Dispatchers;
using Expenso.Shared.System.Types.Messages.Interfaces;

namespace Expenso.IAM.Core.Users.Proxy;

internal sealed class IamProxy(IQueryDispatcher queryDispatcher, IMessageContextFactory messageContextFactory)
    : IIamProxy
{
    private readonly IMessageContextFactory _messageContextFactory =
        messageContextFactory ?? throw new ArgumentNullException(nameof(messageContextFactory));

    private readonly IQueryDispatcher _queryDispatcher =
        queryDispatcher ?? throw new ArgumentNullException(nameof(queryDispatcher));

    public async Task<GetUserExternalResponse?> GetUserByIdAsync(string userId,
        CancellationToken cancellationToken = default)
    {
        GetUserResponse? userQueryResponse =
            await _queryDispatcher.QueryAsync(new GetUserQuery(_messageContextFactory.Current(), userId),
                cancellationToken);

        return GetUserExternalResponseMap.MapTo(userQueryResponse);
    }

    public async Task<GetUserExternalResponse?> GetUserByEmailAsync(string email,
        CancellationToken cancellationToken = default)
    {
        GetUserResponse? userQueryResponse =
            await _queryDispatcher.QueryAsync(new GetUserQuery(_messageContextFactory.Current(), Email: email),
                cancellationToken);

        return GetUserExternalResponseMap.MapTo(userQueryResponse);
    }
}
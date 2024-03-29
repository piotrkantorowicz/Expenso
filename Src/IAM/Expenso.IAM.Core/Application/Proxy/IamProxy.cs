using Expenso.IAM.Core.Application.Users.Read.Queries.GetUser;
using Expenso.IAM.Proxy;
using Expenso.IAM.Proxy.DTO.GetUser;
using Expenso.Shared.Queries.Dispatchers;
using Expenso.Shared.System.Types.Messages.Interfaces;

namespace Expenso.IAM.Core.Application.Proxy;

internal sealed class IamProxy(IQueryDispatcher queryDispatcher, IMessageContextFactory messageContextFactory)
    : IIamProxy
{
    private readonly IMessageContextFactory _messageContextFactory =
        messageContextFactory ?? throw new ArgumentNullException(nameof(messageContextFactory));

    private readonly IQueryDispatcher _queryDispatcher =
        queryDispatcher ?? throw new ArgumentNullException(nameof(queryDispatcher));

    public async Task<GetUserResponse?> GetUserByIdAsync(string userId, CancellationToken cancellationToken = default)
    {
        return await _queryDispatcher.QueryAsync(new GetUserQuery(_messageContextFactory.Current(), userId),
            cancellationToken);
    }

    public async Task<GetUserResponse?> GetUserByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return await _queryDispatcher.QueryAsync(new GetUserQuery(_messageContextFactory.Current(), Email: email),
            cancellationToken);
    }
}
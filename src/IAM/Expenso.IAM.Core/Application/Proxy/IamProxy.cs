using Expenso.IAM.Core.Application.Users.Read.Queries.GetUser;
using Expenso.IAM.Shared;
using Expenso.IAM.Shared.DTO.GetUser;
using Expenso.Shared.Queries.Dispatchers;
using Expenso.Shared.System.Types.Messages.Interfaces;

namespace Expenso.IAM.Core.Application.Proxy;

internal sealed class IamProxy : IIamProxy
{
    private readonly IMessageContextFactory _messageContextFactory;
    private readonly IQueryDispatcher _queryDispatcher;

    public IamProxy(IQueryDispatcher queryDispatcher, IMessageContextFactory messageContextFactory)
    {
        _messageContextFactory = messageContextFactory ??
                                 throw new ArgumentNullException(paramName: nameof(messageContextFactory));

        _queryDispatcher = queryDispatcher ?? throw new ArgumentNullException(paramName: nameof(queryDispatcher));
    }

    public async Task<GetUserResponse?> GetUserByIdAsync(string userId, CancellationToken cancellationToken = default)
    {
        return await _queryDispatcher.QueryAsync(
            query: new GetUserQuery(MessageContext: _messageContextFactory.Current(), UserId: userId),
            cancellationToken: cancellationToken);
    }

    public async Task<GetUserResponse?> GetUserByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return await _queryDispatcher.QueryAsync(
            query: new GetUserQuery(MessageContext: _messageContextFactory.Current(), Email: email),
            cancellationToken: cancellationToken);
    }
}
using Expenso.IAM.Core.Application.Users.Read.Queries.GetUserByEmail;
using Expenso.IAM.Core.Application.Users.Read.Queries.GetUserById;
using Expenso.IAM.Shared;
using Expenso.IAM.Shared.DTO.GetUserByEmail.Request;
using Expenso.IAM.Shared.DTO.GetUserByEmail.Response;
using Expenso.IAM.Shared.DTO.GetUserById.Request;
using Expenso.IAM.Shared.DTO.GetUserById.Response;
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

    public async Task<GetUserByIdResponse?> GetUserByIdAsync(string userId,
        CancellationToken cancellationToken = default)
    {
        return await _queryDispatcher.QueryAsync(query: new GetUserByIdQuery(
            MessageContext: _messageContextFactory.Current(),
                Payload: new GetUserByIdRequest(UserId: userId)), cancellationToken: cancellationToken);
    }

    public async Task<GetUserByEmailResponse?> GetUserByEmailAsync(string email,
        CancellationToken cancellationToken = default)
    {
        return await _queryDispatcher.QueryAsync(query: new GetUserByEmailQuery(
            MessageContext: _messageContextFactory.Current(),
                Payload: new GetUserByEmailRequest(Email: email)), cancellationToken: cancellationToken);
    }
}
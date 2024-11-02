using Expenso.IAM.Core.Application.Users.Read.Queries.GetUserByEmail;
using Expenso.IAM.Core.Application.Users.Read.Queries.GetUserById;
using Expenso.IAM.Core.Application.Users.Read.Queries.GetUsers;
using Expenso.IAM.Shared;
using Expenso.IAM.Shared.DTO.GetUserByEmail.Request;
using Expenso.IAM.Shared.DTO.GetUserByEmail.Response;
using Expenso.IAM.Shared.DTO.GetUserById.Request;
using Expenso.IAM.Shared.DTO.GetUserById.Response;
using Expenso.IAM.Shared.DTO.GetUsers.Request;
using Expenso.IAM.Shared.DTO.GetUsers.Response;
using Expenso.Shared.Queries.Dispatchers;
using Expenso.Shared.System.Modules.Constants;
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

    public async Task<GetUserByIdResponse?> GetUserByIdAsync(GetUserByIdRequest request, IMessageContext? messageContext = null,
        CancellationToken cancellationToken = default)
    {
        return await _queryDispatcher.QueryAsync(
            query: new GetUserByIdQuery(MessageContext:             messageContext is null
            ? _messageContextFactory.Current(moduleId: Names.IamModule)
            : _messageContextFactory.FromParent(parent: messageContext, moduleId: Names.IamModule), Payload: request),
            cancellationToken: cancellationToken);
    }

    public async Task<GetUserByEmailResponse?> GetUserByEmailAsync(GetUserByEmailRequest request, IMessageContext? messageContext = null,
        CancellationToken cancellationToken = default)
    {
        return await _queryDispatcher.QueryAsync(
            query: new GetUserByEmailQuery(            MessageContext: messageContext is null
                ? _messageContextFactory.Current(moduleId: Names.IamModule)
                : _messageContextFactory.FromParent(parent: messageContext, moduleId: Names.IamModule), Payload: request),
            cancellationToken: cancellationToken);
    }

    public async Task<IReadOnlyCollection<GetUsersResponse>?> GetUsersAsync(GetUsersRequest request, IMessageContext? messageContext = null,
        CancellationToken cancellationToken = default)
    {
        return await _queryDispatcher.QueryAsync(
            query: new GetUsersQuery(            MessageContext: messageContext is null
                ? _messageContextFactory.Current(moduleId: Names.IamModule)
                : _messageContextFactory.FromParent(parent: messageContext, moduleId: Names.IamModule), Payload: request),
            cancellationToken: cancellationToken);
    }
}
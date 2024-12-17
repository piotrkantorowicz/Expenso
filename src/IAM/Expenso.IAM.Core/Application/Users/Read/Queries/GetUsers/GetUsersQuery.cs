using Expenso.IAM.Shared.DTO.GetUsers.Request;
using Expenso.IAM.Shared.DTO.GetUsers.Response;
using Expenso.Shared.Queries;
using Expenso.Shared.System.Types.Messages.Interfaces;

namespace Expenso.IAM.Core.Application.Users.Read.Queries.GetUsers;

internal sealed record GetUsersQuery(IMessageContext MessageContext, GetUsersRequest? Payload)
    : IQuery<IReadOnlyCollection<GetUsersResponse>>;
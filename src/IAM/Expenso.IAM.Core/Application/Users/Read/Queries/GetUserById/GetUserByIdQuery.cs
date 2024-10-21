using Expenso.IAM.Shared.DTO.GetUserById.Request;
using Expenso.IAM.Shared.DTO.GetUserById.Response;
using Expenso.Shared.Queries;
using Expenso.Shared.System.Types.Messages.Interfaces;

namespace Expenso.IAM.Core.Application.Users.Read.Queries.GetUserById;

internal sealed record GetUserByIdQuery(IMessageContext MessageContext, GetUserByIdRequest? Payload)
    : IQuery<GetUserByIdResponse>;
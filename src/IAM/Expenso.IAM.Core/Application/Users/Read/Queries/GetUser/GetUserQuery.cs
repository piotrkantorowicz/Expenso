using Expenso.IAM.Shared.DTO.GetUser.Request;
using Expenso.IAM.Shared.DTO.GetUser.Response;
using Expenso.Shared.Queries;
using Expenso.Shared.System.Types.Messages.Interfaces;

namespace Expenso.IAM.Core.Application.Users.Read.Queries.GetUser;

internal sealed record GetUserQuery(IMessageContext MessageContext, GetUserRequest? Payload) : IQuery<GetUserResponse>;
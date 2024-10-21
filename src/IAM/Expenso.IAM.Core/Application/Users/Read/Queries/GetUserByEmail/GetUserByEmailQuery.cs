using Expenso.IAM.Shared.DTO.GetUserByEmail.Request;
using Expenso.IAM.Shared.DTO.GetUserByEmail.Response;
using Expenso.Shared.Queries;
using Expenso.Shared.System.Types.Messages.Interfaces;

namespace Expenso.IAM.Core.Application.Users.Read.Queries.GetUserByEmail;

internal sealed record GetUserByEmailQuery(IMessageContext MessageContext, GetUserByEmailRequest? Payload)
    : IQuery<GetUserByEmailResponse>;
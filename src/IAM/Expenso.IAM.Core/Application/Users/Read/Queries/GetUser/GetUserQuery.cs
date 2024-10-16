using Expenso.IAM.Shared.DTO.GetUser;
using Expenso.Shared.Queries;
using Expenso.Shared.System.Types.Messages.Interfaces;

namespace Expenso.IAM.Core.Application.Users.Read.Queries.GetUser;

internal sealed record GetUserQuery(IMessageContext MessageContext, string? UserId = null, string? Email = null)
    : IQuery<GetUserResponse>;
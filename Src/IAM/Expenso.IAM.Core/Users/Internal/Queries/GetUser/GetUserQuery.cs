using Expenso.IAM.Proxy.DTO.GetUser;
using Expenso.Shared.Queries;
using Expenso.Shared.System.Types.Messages.Interfaces;

namespace Expenso.IAM.Core.Users.Internal.Queries.GetUser;

internal sealed record GetUserQuery(IMessageContext MessageContext, string? UserId = null, string? Email = null)
    : IQuery<GetUserInternalResponse>;
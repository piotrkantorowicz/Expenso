using Expenso.IAM.Core.Users.Queries.GetUser.DTO.Response;
using Expenso.Shared.Queries;
using Expenso.Shared.System.Types.Messages.Interfaces;

namespace Expenso.IAM.Core.Users.Queries.GetUser;

internal sealed record GetUserQuery(IMessageContext MessageContext, string? UserId = null, string? Email = null)
    : IQuery<GetUserResponse>;
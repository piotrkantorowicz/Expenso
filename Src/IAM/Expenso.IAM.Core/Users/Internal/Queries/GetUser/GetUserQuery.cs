using Expenso.IAM.Proxy.DTO.GetUser;
using Expenso.Shared.Queries;

namespace Expenso.IAM.Core.Users.Internal.Queries.GetUser;

internal sealed record GetUserQuery(string? UserId = null, string? Email = null) : IQuery<GetUserInternalResponse>;
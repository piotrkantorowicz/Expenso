using Expenso.IAM.Proxy.DTO.GetUser;
using Expenso.Shared.Queries;

namespace Expenso.IAM.Core.Users.Queries.GetUserInternal;

internal sealed record GetUserInternalQuery(string? Id = null, string? Email = null) : IQuery<GetUserInternalResponse>;
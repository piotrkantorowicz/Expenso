using Expenso.IAM.Core.Users.DTO.GetUser;
using Expenso.Shared.Queries;

namespace Expenso.IAM.Core.Users.Queries.GetUser;

internal sealed record GetUserQuery(string? UserId = null, string? Email = null) : IQuery<GetUserResponse>;
using Expenso.IAM.Core.Users.DTO.GetUser;
using Expenso.Shared.Queries;

namespace Expenso.IAM.Core.Users.Queries.GetUser;

internal sealed record GetUserQuery(string? Id = null, string? Email = null) : IQuery<GetUserResponse>;
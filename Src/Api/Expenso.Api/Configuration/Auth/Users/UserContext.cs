using Expenso.Shared.System.Types.ExecutionContext.Models;

namespace Expenso.Api.Configuration.Auth.Users;

internal sealed record UserContext(string? UserId, string? Username) : IUserContext;
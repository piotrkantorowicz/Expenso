using Expenso.Shared.System.Types.UserContext;

namespace Expenso.Api.Configuration.Auth.Users.UserContext;

internal sealed record UserContext(string? UserId, string? Username) : IUserContext;
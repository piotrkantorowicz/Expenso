namespace Expenso.Shared.System.Types.ExecutionContext.Models;

public interface IUserContext
{
    string? UserId { get; }

    string? Username { get; }
}
namespace Expenso.UserPreferences.Core.Models;

internal sealed record UserId(Guid Value)
{
    public static implicit operator Guid(UserId id) => id.Value;
    public static implicit operator UserId?(Guid id) => 
        id.Equals(Guid.Empty) ? null : new UserId(id);
}
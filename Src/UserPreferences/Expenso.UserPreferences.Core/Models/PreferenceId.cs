namespace Expenso.UserPreferences.Core.Models;

internal sealed record PreferenceId(Guid Value)
{
    public static implicit operator Guid(PreferenceId id) => id.Value;
    public static implicit operator PreferenceId?(Guid id) => 
        id.Equals(Guid.Empty) ? null : new PreferenceId(id);
}
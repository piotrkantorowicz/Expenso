namespace Expenso.Shared.System.Configuration.Exceptions;

public sealed class SettingsHasNotBeenBoundYetException : Exception
{
    public SettingsHasNotBeenBoundYetException(string settingsName) : base(
        message: $"Settings of type {settingsName} have not been bound yet.")
    {
    }
}
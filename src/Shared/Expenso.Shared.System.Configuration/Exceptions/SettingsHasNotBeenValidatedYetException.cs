namespace Expenso.Shared.System.Configuration.Exceptions;

public sealed class SettingsHasNotBeenValidatedYetException : Exception
{
    public SettingsHasNotBeenValidatedYetException(string settingsName) : base(
        message: $"Settings of type {settingsName} have not been validated yet.")
    {
    }
}
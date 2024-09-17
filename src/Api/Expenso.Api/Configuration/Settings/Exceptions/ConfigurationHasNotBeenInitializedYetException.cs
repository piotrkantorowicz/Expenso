namespace Expenso.Api.Configuration.Settings.Exceptions;

public sealed class ConfigurationHasNotBeenInitializedYetException : Exception
{
    public ConfigurationHasNotBeenInitializedYetException() : base(
        message:
        "Configuration has not been initialized yet. To resolve this issue, create a new instance of AppConfigurationManager or just call the AppConfigurationManager.Configure() method.")
    {
    }
}
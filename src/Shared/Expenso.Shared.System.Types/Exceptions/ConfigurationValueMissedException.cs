namespace Expenso.Shared.System.Types.Exceptions;

public sealed class ConfigurationValueMissedException : Exception
{
    public ConfigurationValueMissedException(string key) : base(
        message:
        $"The configuration value for key '{key}' is missing. Please ensure the value is provided in the configuration.")
    {
    }
}
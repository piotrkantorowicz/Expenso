namespace Expenso.Api.Configuration.Settings.Exceptions;

internal sealed class InvalidSettingsTypeException : Exception
{
    public InvalidSettingsTypeException() : base(
        message: "The requested settings type does not match the actual type stored in the configuration.")
    {
    }
}
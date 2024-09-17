namespace Expenso.Api.Configuration.Settings.Exceptions;

public sealed class InvalidSettingsTypeException : Exception
{
    public InvalidSettingsTypeException() : base(
        message: "The requested settings type does not match the actual type stored in the configuration.")
    {
    }
}
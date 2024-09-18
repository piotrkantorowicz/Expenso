namespace Expenso.Api.Configuration.Settings.Exceptions;

public sealed class MissingSettingsSectionException : Exception
{
    public MissingSettingsSectionException(string sectionName) : base(
        message:
        $"The settings section '{sectionName}' is missing. Please ensure the section is properly configured in the configuration file.")
    {
    }
}
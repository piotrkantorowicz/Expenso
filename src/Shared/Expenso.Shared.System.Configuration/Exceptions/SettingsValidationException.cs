using Expenso.Shared.System.Types.Exceptions.Validation;

namespace Expenso.Shared.System.Configuration.Exceptions;

public class SettingsValidationException : ValidationException
{
    public SettingsValidationException(IDictionary<string, string> errorDictionary) : base(
        errorDictionary: errorDictionary)
    {
    }
}
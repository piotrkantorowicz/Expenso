using Expenso.Shared.System.Types.Exceptions;

namespace Expenso.Shared.Commands.Validation.Exceptions;

public sealed class CommandValidationException : ValidationException
{
    public CommandValidationException(IDictionary<string, string> errorDictionary) : base(
        errorDictionary: errorDictionary)
    {
    }
}
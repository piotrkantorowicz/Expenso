using Expenso.Shared.System.Types.Exceptions;
using Expenso.Shared.System.Types.Exceptions.Validation;

namespace Expenso.Shared.Commands.Validation;

public sealed class CommandValidationException : ValidationException
{
    public CommandValidationException(IDictionary<string, string> errorDictionary) : base(
        errorDictionary: errorDictionary)
    {
    }
}
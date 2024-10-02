using Expenso.Shared.System.Types.TypesExtensions;

namespace Expenso.Shared.Commands.Validation.Helpers;

public static class ValidationHelper
{
    public static void ValidateAll<TCommand>(IDictionary<string, string> errors,
        IReadOnlyDictionary<string, CommandValidationRule<TCommand>[]> validationMetadata, TCommand? command)
        where TCommand : class, ICommand
    {
        foreach (KeyValuePair<string, CommandValidationRule<TCommand>[]> validationRule in validationMetadata)
        {
            Validate(errors: errors, validationMetadata: validationMetadata, property: validationRule.Key,
                command: command);
        }
    }

    public static void Validate<TCommand>(IDictionary<string, string> errors,
        IReadOnlyDictionary<string, CommandValidationRule<TCommand>[]> validationMetadata, string property,
        TCommand? command) where TCommand : class, ICommand
    {
        foreach (CommandValidationRule<TCommand> validationRule in validationMetadata[key: property])
        {
            (string Property, string ErrorMessage)? error =
                validationRule.Validate(command: command, property: property);

            if (error is not null)
            {
                errors.AddTuple(item: error);
            }
        }
    }
}
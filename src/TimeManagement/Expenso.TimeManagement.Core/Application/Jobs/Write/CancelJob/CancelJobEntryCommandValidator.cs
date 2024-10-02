using Expenso.Shared.Commands.Validation;
using Expenso.Shared.Commands.Validation.Helpers;
using Expenso.Shared.Commands.Validation.Utils;

namespace Expenso.TimeManagement.Core.Application.Jobs.Write.CancelJob;

internal sealed class CancelJobEntryCommandValidator : ICommandValidator<CancelJobEntryCommand>
{
    private readonly IReadOnlyDictionary<string, CommandValidationRule<CancelJobEntryCommand>[]> _validationMetadata =
        new Dictionary<string, CommandValidationRule<CancelJobEntryCommand>[]>
        {
            {
                ValidationUtils.Command, [
                    new CommandValidationRule<CancelJobEntryCommand>(validationFailedFunc: command => command is null,
                        createMessageFunc: _ => "Command is required.", validationType: ValidationTypes.Required,
                        value: true)
                ]
            },
            {
                nameof(CancelJobEntryCommand.CancelJobEntryRequest), [
                    new CommandValidationRule<CancelJobEntryCommand>(
                        validationFailedFunc: command => command?.CancelJobEntryRequest is null,
                        createMessageFunc: _ => "Cancel job entry request is required.",
                        validationType: ValidationTypes.Required, value: true)
                ]
            },
            {
                nameof(CancelJobEntryCommand.CancelJobEntryRequest.JobEntryId), [
                    new CommandValidationRule<CancelJobEntryCommand>(
                        validationFailedFunc: command =>
                            command?.CancelJobEntryRequest?.JobEntryId is null ||
                            command.CancelJobEntryRequest?.JobEntryId == Guid.Empty,
                        createMessageFunc: _ => "JobEntryId is required.", validationType: ValidationTypes.Required,
                        value: true)
                ]
            }
        };

    public IReadOnlyDictionary<string, CommandValidationRule<CancelJobEntryCommand>[]> GetValidationMetadata()
    {
        return _validationMetadata.ToDictionary(keySelector: kvp => kvp.Key, elementSelector: kvp => kvp.Value);
    }

    public IDictionary<string, string> Validate(CancelJobEntryCommand? command)
    {
        Dictionary<string, string> errors = new();
        ValidationHelper.ValidateAll(errors: errors, validationMetadata: _validationMetadata, command: command);

        return errors;
    }
}
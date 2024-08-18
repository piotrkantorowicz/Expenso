using Expenso.Shared.Commands.Validation;

namespace Expenso.TimeManagement.Core.Application.Jobs.Write.CancelJob;

internal sealed class CancelJobEntryCommandValidator : ICommandValidator<CancelJobEntryCommand>
{
    public IDictionary<string, string> Validate(CancelJobEntryCommand? command)
    {
        Dictionary<string, string> errors = new();

        if (command is null)
        {
            errors.Add(key: nameof(command), value: "Command is required");

            return errors;
        }

        if (command.CancelJobEntryRequest is null)
        {
            errors.Add(key: nameof(command.CancelJobEntryRequest), value: "Cancel job entry request is required");
        }

        if (command.CancelJobEntryRequest?.JobEntryId is null ||
            command.CancelJobEntryRequest?.JobEntryId == Guid.Empty)
        {
            errors.Add(key: nameof(command.CancelJobEntryRequest.JobEntryId), value: "JobEntryId is required");
        }

        return errors;
    }
}
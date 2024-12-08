using Expenso.Shared.Commands.Validation.Validators;
using Expenso.TimeManagement.Core.Application.Jobs.Write.CancelJobEntry.DTO.Request.Validators;

using FluentValidation;

namespace Expenso.TimeManagement.Core.Application.Jobs.Write.CancelJobEntry;

internal sealed class CancelJobEntryCommandValidator : CommandValidator<CancelJobEntryCommand>
{
    public CancelJobEntryCommandValidator(MessageContextValidator messageContextValidator,
        CancelJobEntryRequestValidator cancelJobEntryRequestValidator) : base(
        messageContextValidator: messageContextValidator)
    {
        ArgumentNullException.ThrowIfNull(argument: cancelJobEntryRequestValidator,
            paramName: nameof(cancelJobEntryRequestValidator));

        RuleFor(expression: x => x.Payload)
            .NotNull()
            .WithMessage(errorMessage: "The command payload must not be null.")
            .SetValidator(validator: cancelJobEntryRequestValidator!);
    }
}
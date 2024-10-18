using Expenso.Shared.Commands.Validation.Validators;
using Expenso.TimeManagement.Core.Application.Jobs.Write.RegisterJob.DTO.Request.Validators;

using FluentValidation;

namespace Expenso.TimeManagement.Core.Application.Jobs.Write.RegisterJob;

internal sealed class RegisterJobEntryCommandValidator : CommandValidator<RegisterJobEntryCommand>
{
    public RegisterJobEntryCommandValidator(MessageContextValidator messageContextValidator,
        RegisterJobEntryRequestValidator registerJobEntryRequestValidator) : base(
        messageContextValidator: messageContextValidator)
    {
        ArgumentNullException.ThrowIfNull(argument: registerJobEntryRequestValidator,
            paramName: nameof(registerJobEntryRequestValidator));

        RuleFor(expression: x => x.Payload!)
            .NotNull()
            .WithMessage(errorMessage: "The command payload must not be null.")
            .SetValidator(validator: registerJobEntryRequestValidator);
    }
}
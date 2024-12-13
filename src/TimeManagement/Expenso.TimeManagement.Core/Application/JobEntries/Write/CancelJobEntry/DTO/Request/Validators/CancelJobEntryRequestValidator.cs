using FluentValidation;

namespace Expenso.TimeManagement.Core.Application.JobEntries.Write.CancelJobEntry.DTO.Request.Validators;

internal sealed class CancelJobEntryRequestValidator : AbstractValidator<CancelJobEntryRequest>
{
    public CancelJobEntryRequestValidator()
    {
        RuleFor(expression: x => x.JobEntryId)
            .NotEmpty()
            .WithMessage(errorMessage: "The job entry id must not be null or empty.");
    }
}
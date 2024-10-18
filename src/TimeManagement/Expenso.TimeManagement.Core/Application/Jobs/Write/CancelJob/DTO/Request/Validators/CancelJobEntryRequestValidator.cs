using Expenso.Shared.Commands.Validation.Rules;

using FluentValidation;

namespace Expenso.TimeManagement.Core.Application.Jobs.Write.CancelJob.DTO.Request.Validators;

internal sealed class CancelJobEntryRequestValidator : AbstractValidator<CancelJobEntryRequest>
{
    public CancelJobEntryRequestValidator()
    {
        RuleFor(expression: x => x.JobEntryId)
            .NotNullOrEmpty()
            .WithMessage(errorMessage: "The job entry id must not be null or empty.");
    }
}
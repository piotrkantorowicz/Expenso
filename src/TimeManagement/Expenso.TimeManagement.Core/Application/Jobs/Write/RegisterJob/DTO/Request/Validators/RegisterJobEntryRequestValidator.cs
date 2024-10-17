using Expenso.Shared.System.Types.Clock;
using Expenso.TimeManagement.Shared.DTO.Request;

using FluentValidation;

namespace Expenso.TimeManagement.Core.Application.Jobs.Write.RegisterJob.DTO.Request.Validators;

internal sealed class RegisterJobEntryRequestValidator : AbstractValidator<RegisterJobEntryRequest>
{
    public RegisterJobEntryRequestValidator(
        RegisterJobEntryRequest_JobEntryPeriodIntervalValidator jobEntryPeriodIntervalValidator,
        RegisterJobEntryRequest_JobEntryTriggerValidator jobEntryTriggerValidator, IClock clock)
    {
        ArgumentNullException.ThrowIfNull(argument: jobEntryPeriodIntervalValidator,
            paramName: nameof(jobEntryPeriodIntervalValidator));

        ArgumentNullException.ThrowIfNull(argument: jobEntryTriggerValidator,
            paramName: nameof(jobEntryTriggerValidator));

        ArgumentNullException.ThrowIfNull(argument: clock, paramName: nameof(clock));

        RuleFor(expression: x => x.MaxRetries)
            .NotNull()
            .WithMessage(errorMessage: "Max retries for job entry must not be empty.")
            .GreaterThan(valueToCompare: 0)
            .WithMessage(errorMessage: "Max retries for job entry must be greater than 0.");

        RuleFor(expression: x => x)
            .Must(predicate: x => x.Interval is not null || x.RunAt is not null)
            .WithMessage(
                errorMessage:
                "At least one value must be provided: Interval for periodic jobs or RunAt for single run jobs.")
            .Must(predicate: x => x.Interval is null || x.RunAt is null)
            .WithMessage(errorMessage: "RunAt and Interval cannot be used together.");

        When(predicate: x => x.Interval is not null, action: () =>
        {
            RuleFor(expression: x => x.Interval).SetValidator(validator: jobEntryPeriodIntervalValidator!);
        });

        When(predicate: x => x.RunAt is not null, action: () =>
        {
            RuleFor(expression: x => x.RunAt)
                .Must(predicate: runAt => runAt >= clock.UtcNow)
                .WithMessage(messageProvider: x =>
                    $"RunAt must be greater than current time. Provided: {x.RunAt}. Current: {clock.UtcNow}.");
        });

        RuleFor(expression: x => x.JobEntryTriggers)
            .NotNull()
            .WithMessage(errorMessage: "Job entry triggers must not be empty.")
            .NotEmpty()
            .WithMessage(errorMessage: "Job entry triggers are required.")
            .ForEach(action: x => x.SetValidator(validator: jobEntryTriggerValidator));
    }
}
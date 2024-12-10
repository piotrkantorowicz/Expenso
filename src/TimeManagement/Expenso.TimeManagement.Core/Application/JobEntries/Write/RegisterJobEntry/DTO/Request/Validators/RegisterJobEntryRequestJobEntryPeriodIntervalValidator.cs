using Expenso.TimeManagement.Shared.DTO.RegisterJobEntry.Request;

using FluentValidation;

using NCrontab;

namespace Expenso.TimeManagement.Core.Application.JobEntries.Write.RegisterJobEntry.DTO.Request.Validators;

internal sealed class
    RegisterJobEntryRequestJobEntryPeriodIntervalValidator : AbstractValidator<
    RegisterJobEntryRequestJobEntryPeriodInterval>
{
    public RegisterJobEntryRequestJobEntryPeriodIntervalValidator()
    {
        RuleFor(expression: x => x)
            .Custom(action: (interval, context) =>
            {
                try
                {
                    string cronExpression = interval.GetCronExpression();

                    CrontabSchedule.Parse(expression: cronExpression, options: new CrontabSchedule.ParseOptions
                    {
                        IncludingSeconds = interval.UseSeconds
                    });
                }
                catch (CrontabException crontabException)
                {
                    context.AddFailure(
                        errorMessage: $"Unable to parse provided interval, because of {crontabException.Message}");
                }
            });
    }
}
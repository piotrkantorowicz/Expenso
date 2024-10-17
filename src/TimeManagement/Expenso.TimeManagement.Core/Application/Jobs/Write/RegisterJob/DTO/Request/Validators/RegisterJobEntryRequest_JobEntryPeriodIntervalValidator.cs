using Expenso.TimeManagement.Shared.DTO.Request;

using FluentValidation;

using NCrontab;

namespace Expenso.TimeManagement.Core.Application.Jobs.Write.RegisterJob.DTO.Request.Validators;

internal sealed class
    RegisterJobEntryRequest_JobEntryPeriodIntervalValidator : AbstractValidator<
    RegisterJobEntryRequest_JobEntryPeriodInterval>
{
    public RegisterJobEntryRequest_JobEntryPeriodIntervalValidator()
    {
        RuleFor(expression: x => x)
            .Custom(action: (interval, context) =>
            {
                try
                {
                    string cronExpression = interval!.GetCronExpression();

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
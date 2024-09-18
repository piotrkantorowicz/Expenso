using Expenso.Shared.Commands.Validation;
using Expenso.Shared.System.Serialization;
using Expenso.Shared.System.Serialization.Default;
using Expenso.Shared.System.Types.Clock;
using Expenso.TimeManagement.Proxy.DTO.Request;

using Humanizer;

using NCrontab;

namespace Expenso.TimeManagement.Core.Application.Jobs.Write.RegisterJob;

internal sealed class RegisterJobEntryCommandValidator : ICommandValidator<RegisterJobEntryCommand>
{
    private readonly IClock _clock;
    private readonly ISerializer _serializer;

    public RegisterJobEntryCommandValidator(ISerializer serializer, IClock clock)
    {
        _serializer = serializer ?? throw new ArgumentNullException(paramName: nameof(serializer));
        _clock = clock ?? throw new ArgumentNullException(paramName: nameof(clock));
    }

    public IDictionary<string, string> Validate(RegisterJobEntryCommand? command)
    {
        Dictionary<string, string> errors = new();

        if (command is null)
        {
            errors.Add(key: nameof(command).Pascalize(), value: "Command is required");

            return errors;
        }

        if (command.RegisterJobEntryRequest is null)
        {
            errors.Add(key: nameof(command.RegisterJobEntryRequest), value: "Register job entry request is required");
        }

        if (command.RegisterJobEntryRequest?.MaxRetries is not null && command.RegisterJobEntryRequest?.MaxRetries <= 0)
        {
            errors.Add(key: nameof(command.RegisterJobEntryRequest.MaxRetries),
                value: "MaxRetries must be a positive value");
        }

        if (command.RegisterJobEntryRequest?.Interval is null && command.RegisterJobEntryRequest?.RunAt is null)
        {
            errors.Add(
                key:
                $"{nameof(command.RegisterJobEntryRequest.Interval)}|{nameof(command.RegisterJobEntryRequest.RunAt)}",
                value: "At least one value must be provided: Interval for periodic jobs or RunAt for single run jobs");
        }

        if (command.RegisterJobEntryRequest?.Interval is not null && command.RegisterJobEntryRequest?.RunAt is not null)
        {
            errors.Add(
                key:
                $"{nameof(command.RegisterJobEntryRequest.Interval)}|{nameof(command.RegisterJobEntryRequest.RunAt)}",
                value: "RunAt and Interval cannot be used together");
        }

        if (command.RegisterJobEntryRequest?.Interval is not null)
        {
            try
            {
                string cronExpression = command.RegisterJobEntryRequest.Interval.GetCronExpression();

                CrontabSchedule.Parse(expression: cronExpression, options: new CrontabSchedule.ParseOptions
                {
                    IncludingSeconds = command.RegisterJobEntryRequest.Interval.UseSeconds
                });
            }
            catch (CrontabException crontabException)
            {
                errors.Add(key: nameof(command.RegisterJobEntryRequest.Interval),
                    value: $"Unable to parse provided interval, because of {crontabException.Message}");
            }
        }

        if (command.RegisterJobEntryRequest?.RunAt is not null && command.RegisterJobEntryRequest.RunAt < _clock.UtcNow)
        {
            errors.Add(key: nameof(command.RegisterJobEntryRequest.RunAt),
                value:
                $"RunAt must be greater than current time. Provided: {command.RegisterJobEntryRequest.RunAt}. Current: {_clock.UtcNow}");
        }

        ICollection<RegisterJobEntryRequest_JobEntryTrigger>? jobEntryTriggers =
            command.RegisterJobEntryRequest?.JobEntryTriggers;

        if (jobEntryTriggers is null || jobEntryTriggers.Count is 0)
        {
            errors.Add(key: nameof(command.RegisterJobEntryRequest.JobEntryTriggers),
                value: "Job entry triggers are required");
        }

        if (errors.Count > 0)
        {
            return errors;
        }

        foreach (RegisterJobEntryRequest_JobEntryTrigger jobEntryTrigger in jobEntryTriggers!)
        {
            if (string.IsNullOrEmpty(value: jobEntryTrigger.EventType))
            {
                errors.Add(key: nameof(jobEntryTrigger.EventType), value: "Event type is required");
            }

            if (string.IsNullOrEmpty(value: jobEntryTrigger.EventData))
            {
                errors.Add(key: nameof(jobEntryTrigger.EventData), value: "Event data is required");
            }

            if (jobEntryTrigger.EventType is not null && _serializer.Deserialize(value: jobEntryTrigger.EventData!,
                    type: Type.GetType(typeName: jobEntryTrigger.EventType),
                    settings: DefaultSerializerOptions.DefaultSettings) is null)
            {
                errors.Add(key: $"{nameof(jobEntryTrigger.EventType)}|{nameof(jobEntryTrigger.EventData)}",
                    value: "EventData must be serializable to provided EventType");
            }
        }

        return errors;
    }
}
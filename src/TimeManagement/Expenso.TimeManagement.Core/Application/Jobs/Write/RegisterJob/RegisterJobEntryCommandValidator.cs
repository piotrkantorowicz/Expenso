using System.Text;

using Expenso.Shared.Commands.Validation;
using Expenso.Shared.System.Serialization;
using Expenso.TimeManagement.Proxy.DTO.Request;

namespace Expenso.TimeManagement.Core.Application.Jobs.Write.RegisterJob;

internal sealed class RegisterJobEntryCommandValidator : ICommandValidator<RegisterJobEntryCommand>
{
    private readonly ISerializer _serializer;

    public RegisterJobEntryCommandValidator(ISerializer serializer)
    {
        _serializer = serializer ?? throw new ArgumentNullException(paramName: nameof(serializer));
    }

    public IDictionary<string, string> Validate(RegisterJobEntryCommand? command)
    {
        Dictionary<string, string> errors = new();

        if (command is null)
        {
            errors.Add(key: nameof(command), value: "Command is required");

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
                key: new StringBuilder()
                    .Append(value: nameof(command.RegisterJobEntryRequest.Interval))
                    .Append(value: '|')
                    .Append(value: nameof(command.RegisterJobEntryRequest.RunAt))
                    .ToString(),
                value: "At least one value must be provide Interval for periodic jobs or RunAt for single run jobs");
        }

        if (command.RegisterJobEntryRequest?.Interval is not null && command.RegisterJobEntryRequest?.RunAt is not null)
        {
            errors.Add(
                key: new StringBuilder()
                    .Append(value: nameof(command.RegisterJobEntryRequest.Interval))
                    .Append(value: '|')
                    .Append(value: nameof(command.RegisterJobEntryRequest.RunAt))
                    .ToString(), value: "RunAt and Interval cannot be used together");
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
                    type: Type.GetType(typeName: jobEntryTrigger.EventType)) is null)
            {
                errors.Add(
                    key: new StringBuilder()
                        .Append(value: nameof(jobEntryTrigger.EventType))
                        .Append(value: '|')
                        .Append(value: nameof(jobEntryTrigger.EventData))
                        .ToString(), value: "EventData must be serializable to provided EventType");
            }
        }

        return errors;
    }
}
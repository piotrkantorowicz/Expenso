using System.Text;

using Expenso.Shared.Commands.Validation;
using Expenso.Shared.System.Serialization;
using Expenso.TimeManagement.Proxy.DTO.Request;

namespace Expenso.TimeManagement.Core.Application.Jobs.Write.RegisterJob;

internal sealed class RegisterJobCommandValidator : ICommandValidator<RegisterJobCommand>
{
    private readonly ISerializer _serializer;

    public RegisterJobCommandValidator(ISerializer serializer)
    {
        _serializer = serializer ?? throw new ArgumentNullException(paramName: nameof(serializer));
    }

    public IDictionary<string, string> Validate(RegisterJobCommand? command)
    {
        Dictionary<string, string> errors = new();

        if (command is null)
        {
            errors.Add(key: nameof(command), value: "Command is required");

            return errors;
        }

        if (command.AddJobEntryRequest is null)
        {
            errors.Add(key: nameof(command.AddJobEntryRequest), value: "Add job entry request is required");
        }

        if (command.AddJobEntryRequest?.MaxRetries is not null && command.AddJobEntryRequest?.MaxRetries <= 0)
        {
            errors.Add(key: nameof(command.AddJobEntryRequest.MaxRetries),
                value: "MaxRetries must be a positive value");
        }

        if (command.AddJobEntryRequest?.Interval is null && command.AddJobEntryRequest?.RunAt is null)
        {
            errors.Add(
                key: new StringBuilder()
                    .Append(value: nameof(command.AddJobEntryRequest.Interval))
                    .Append(value: '|')
                    .Append(value: nameof(command.AddJobEntryRequest.RunAt))
                    .ToString(),
                value: "At least one value must be provide Interval for periodic jobs or RunAt for single run jobs");
        }

        if (command.AddJobEntryRequest?.Interval is not null && command.AddJobEntryRequest?.RunAt is not null)
        {
            errors.Add(
                key: new StringBuilder()
                    .Append(value: nameof(command.AddJobEntryRequest.Interval))
                    .Append(value: '|')
                    .Append(value: nameof(command.AddJobEntryRequest.RunAt))
                    .ToString(), value: "RunAt and Interval cannot be used together");
        }

        ICollection<AddJobEntryRequest_JobEntryTrigger>?
            jobEntryTriggers = command.AddJobEntryRequest?.JobEntryTriggers;

        if (jobEntryTriggers is null || jobEntryTriggers.Count is 0)
        {
            errors.Add(key: nameof(command.AddJobEntryRequest.JobEntryTriggers),
                value: "Job entry triggers are required");
        }

        if (errors.Count > 0)
        {
            return errors;
        }

        foreach (AddJobEntryRequest_JobEntryTrigger jobEntryTrigger in jobEntryTriggers!)
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
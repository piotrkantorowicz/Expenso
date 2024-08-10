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
        _serializer = serializer ?? throw new ArgumentNullException(nameof(serializer));
    }

    public IDictionary<string, string> Validate(RegisterJobCommand? command)
    {
        Dictionary<string, string> errors = new();

        if (command is null)
        {
            errors.Add(nameof(command), "Command is required");

            return errors;
        }

        if (command.AddJobEntryRequest is null)
        {
            errors.Add(nameof(command.AddJobEntryRequest), "Add job entry request is required");
        }

        if (command.AddJobEntryRequest?.MaxRetries is not null && command.AddJobEntryRequest?.MaxRetries <= 0)
        {
            errors.Add(nameof(command.AddJobEntryRequest.MaxRetries), "MaxRetries must be a positive value");
        }

        if (command.AddJobEntryRequest?.Interval is null && command.AddJobEntryRequest?.RunAt is null)
        {
            errors.Add(
                new StringBuilder()
                    .Append(nameof(command.AddJobEntryRequest.Interval))
                    .Append('|')
                    .Append(nameof(command.AddJobEntryRequest.RunAt))
                    .ToString(),
                "At least one value must be provide Interval for periodic jobs or RunAt for single run jobs");
        }

        if (command.AddJobEntryRequest?.Interval is not null && command.AddJobEntryRequest?.RunAt is not null)
        {
            errors.Add(
                new StringBuilder()
                    .Append(nameof(command.AddJobEntryRequest.Interval))
                    .Append('|')
                    .Append(nameof(command.AddJobEntryRequest.RunAt))
                    .ToString(), "RunAt and Interval cannot be used together");
        }

        ICollection<AddJobEntryRequest_JobEntryTrigger>?
            jobEntryTriggers = command.AddJobEntryRequest?.JobEntryTriggers;

        if (jobEntryTriggers is null || jobEntryTriggers.Count is 0)
        {
            errors.Add(nameof(command.AddJobEntryRequest.JobEntryTriggers), "Job entry triggers are required");
        }

        if (errors.Count > 0)
            return errors;

        foreach (AddJobEntryRequest_JobEntryTrigger jobEntryTrigger in jobEntryTriggers!)
        {
            if (string.IsNullOrEmpty(jobEntryTrigger.EventType))
            {
                errors.Add(nameof(jobEntryTrigger.EventType), "Event type is required");
            }

            if (string.IsNullOrEmpty(jobEntryTrigger.EventData))
            {
                errors.Add(nameof(jobEntryTrigger.EventData), "Event data is required");
            }

            if (jobEntryTrigger.EventType is not null &&
                _serializer.Deserialize(jobEntryTrigger.EventData!, Type.GetType(jobEntryTrigger.EventType)) is null)
            {
                errors.Add(
                    new StringBuilder()
                        .Append(nameof(jobEntryTrigger.EventType))
                        .Append('|')
                        .Append(nameof(jobEntryTrigger.EventData))
                        .ToString(), "EventData must be serializable to provided EventType");
            }
        }

        return errors;
    }
}
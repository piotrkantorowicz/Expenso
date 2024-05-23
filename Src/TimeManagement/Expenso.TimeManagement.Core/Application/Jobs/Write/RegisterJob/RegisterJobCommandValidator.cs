using Expenso.Shared.Commands.Validation;
using Expenso.TimeManagement.Proxy.DTO.Request;

namespace Expenso.TimeManagement.Core.Application.Jobs.Write.RegisterJob;

internal sealed class RegisterJobCommandValidator : ICommandValidator<RegisterJobCommand>
{
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
        
        if (string.IsNullOrEmpty(command.AddJobEntryRequest?.JobTypeNameCode))
        {
            errors.Add(nameof(command.AddJobEntryRequest.JobTypeNameCode), "Job type name code is required");
        }
        
        ICollection<AddJobEntryRequest_JobEntryPeriod>? jobEntryPeriods = command.AddJobEntryRequest?.JobEntryPeriods;
        
        if (jobEntryPeriods is null || jobEntryPeriods.Count == 0)
        {
            errors.Add(nameof(command.AddJobEntryRequest.JobEntryPeriods), "Job entry periods are required");
        }
        
        ICollection<AddJobEntryRequest_JobEntryTrigger>?
            jobEntryTriggers = command.AddJobEntryRequest?.JobEntryTriggers;
        
        if (jobEntryTriggers is null || jobEntryTriggers.Count == 0)
        {
            errors.Add(nameof(command.AddJobEntryRequest.JobEntryTriggers), "Job entry triggers are required");
        }
        
        if (errors.Count > 0)
            return errors;
        
        foreach (AddJobEntryRequest_JobEntryPeriod jobEntryPeriod in jobEntryPeriods!)
        {
            if (jobEntryPeriod.Interval is null)
            {
                errors.Add(nameof(jobEntryPeriod.Interval), "Interval is required");
            }
        }
        
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
        }
        
        return errors;
    }
}
namespace Expenso.TimeManagement.Proxy.DTO.Request;

public sealed record AddJobEntryRequest(
    string JobTypeName,
    ICollection<AddJobEntryRequest_JobEntryPeriod> JobEntryPeriods,
    ICollection<AddJobEntryRequest_JobEntryTrigger> JobEntryTriggers);
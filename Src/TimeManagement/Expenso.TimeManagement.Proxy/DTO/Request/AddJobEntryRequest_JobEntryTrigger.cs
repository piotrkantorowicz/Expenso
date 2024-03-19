namespace Expenso.TimeManagement.Proxy.DTO.Request;

public record AddJobEntryRequest_JobEntryTrigger(string? EventType, object? EventData);
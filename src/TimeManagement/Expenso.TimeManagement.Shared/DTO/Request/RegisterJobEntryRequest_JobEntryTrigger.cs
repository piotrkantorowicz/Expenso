namespace Expenso.TimeManagement.Shared.DTO.Request;

public sealed record RegisterJobEntryRequest_JobEntryTrigger(
    RegisterJobEntryRequest_JobEntryTrigger_AllowedEventType? EventType,
    string? EventData);
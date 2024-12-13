namespace Expenso.TimeManagement.Shared.DTO.RegisterJobEntry.Request;

public sealed record RegisterJobEntryRequestJobEntryTrigger(
    RegisterJobEntryRequestJobEntryTriggerAllowedEventType? EventType,
    string? EventData);
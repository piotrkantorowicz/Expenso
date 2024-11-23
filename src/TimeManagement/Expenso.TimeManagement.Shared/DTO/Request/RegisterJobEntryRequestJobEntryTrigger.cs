namespace Expenso.TimeManagement.Shared.DTO.Request;

public sealed record RegisterJobEntryRequestJobEntryTrigger(
    RegisterJobEntryRequestJobEntryTriggerAllowedEventType? EventType,
    string? EventData);
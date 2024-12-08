namespace Expenso.TimeManagement.Shared.DTO.GetJobEntry.Response;

public sealed record GetJobEntryResponseJobEntryTrigger(Guid? Id, string? EventType, string? EventData);
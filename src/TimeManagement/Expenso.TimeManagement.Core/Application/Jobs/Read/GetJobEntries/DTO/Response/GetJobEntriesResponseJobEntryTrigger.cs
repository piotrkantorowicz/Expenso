namespace Expenso.TimeManagement.Core.Application.Jobs.Read.GetJobEntries.DTO.Response;

public sealed record GetJobEntriesResponseJobEntryTrigger(Guid? Id, string? EventType, string? EventData);
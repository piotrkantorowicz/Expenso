namespace Expenso.TimeManagement.Core.Application.Jobs.Read.GetJobEntries.DTO.Response;

public sealed record GetJobEntriesResponseJobEntryStatus(Guid? Id, string? Name, string? Description);
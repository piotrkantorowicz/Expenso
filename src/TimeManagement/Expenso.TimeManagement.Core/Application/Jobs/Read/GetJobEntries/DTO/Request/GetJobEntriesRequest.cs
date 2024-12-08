namespace Expenso.TimeManagement.Core.Application.Jobs.Read.GetJobEntries.DTO.Request;

public sealed record GetJobEntriesRequest(
    Guid? JobEntryId = null,
    Guid? JobInstanceId = null,
    Guid[]? JobEntryStatusIds = null,
    int? MoreThanRetries = null,
    bool? IsCompleted = null,
    bool? HasRunned = null,
    bool? IsActive = null,
    bool? HasTriggers = null,
    GetJobEntriesRequestJobEntryIncludes? Includes = null);
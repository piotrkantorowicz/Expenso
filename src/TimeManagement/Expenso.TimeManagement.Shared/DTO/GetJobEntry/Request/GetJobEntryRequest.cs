namespace Expenso.TimeManagement.Shared.DTO.GetJobEntry.Request;

public sealed record GetJobEntryRequest(Guid JobEntryId, GetJobEntryRequestJobEntryIncludes? Includes = null);
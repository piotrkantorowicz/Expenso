namespace Expenso.TimeManagement.Core.Application.JobEntries.Read.GetJobEntries.DTO.Request;

[Flags]
public enum GetJobEntriesRequestJobEntryIncludes
{
    None = 0,
    JobEntryStatus = 1,
    JobEntryInstance = 2,
    All = JobEntryStatus | JobEntryInstance
}
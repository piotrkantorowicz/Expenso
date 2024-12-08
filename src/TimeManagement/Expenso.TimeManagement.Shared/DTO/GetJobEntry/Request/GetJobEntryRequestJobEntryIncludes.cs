namespace Expenso.TimeManagement.Shared.DTO.GetJobEntry.Request;

public enum GetJobEntryRequestJobEntryIncludes
{
    None = 0,
    JobEntryStatus = 1,
    JobEntryInstance = 2,
    All = JobEntryStatus | JobEntryInstance
}
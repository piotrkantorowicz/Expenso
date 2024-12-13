namespace Expenso.TimeManagement.Core.Domain.JobEntries.Repositories.Specifications;

[Flags]
internal enum JobEntryIncludes
{
    None = 0,
    JobEntryStatus = 1,
    JobEntryInstance = 2,
    All = JobEntryStatus | JobEntryInstance
}
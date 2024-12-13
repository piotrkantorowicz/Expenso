namespace Expenso.TimeManagement.Core.Application.JobEntries.Read.GetJobEntries.DTO.Response;

public sealed record GetJobEntriesResponseJobInstance(Guid? Id, string? Name, int? RunningDelay);
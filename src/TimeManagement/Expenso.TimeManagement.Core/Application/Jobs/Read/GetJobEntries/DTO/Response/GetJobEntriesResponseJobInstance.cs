namespace Expenso.TimeManagement.Core.Application.Jobs.Read.GetJobEntries.DTO.Response;

public sealed record GetJobEntriesResponseJobInstance(Guid? Id, string? Name, int? RunningDelay);
namespace Expenso.TimeManagement.Shared.DTO.GetJobEntry.Response;

public sealed record GetJobEntryResponseJobInstance(Guid? Id, string? Name, int? RunningDelay);
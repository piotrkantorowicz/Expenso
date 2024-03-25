namespace Expenso.TimeManagement.Core.Application.Jobs.Shared.Helpers;

public record IntervalPrediction(bool ShouldRun, DateTimeOffset? LastRun);
namespace Expenso.TimeManagement.Core.Application.Jobs.Shared.Helpers;

internal sealed record IntervalPrediction(bool ShouldRun, DateTimeOffset? LastRun);
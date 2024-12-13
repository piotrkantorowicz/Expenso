using Expenso.TimeManagement.Core.Application.Shared.Settings;

namespace Expenso.TimeManagement.Core.Application.JobEntries.Shared.BackgroundJobs.Events;

internal sealed class InvalidEventTypeException : Exception
{
    public InvalidEventTypeException(AllowedEventType type) : base(message: $"Invalid event type: {type}")
    {
    }
}
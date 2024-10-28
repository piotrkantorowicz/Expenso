using Expenso.TimeManagement.Core.Application.Shared.Settings;

namespace Expenso.TimeManagement.Core.Application.Jobs.Shared.BackgroundJobs.Events;

internal sealed class InvalidEventTypeException : Exception
{
    public InvalidEventTypeException(AllowedEventType type) : base(message: $"Invalid event type: {type}")
    {
    }
}
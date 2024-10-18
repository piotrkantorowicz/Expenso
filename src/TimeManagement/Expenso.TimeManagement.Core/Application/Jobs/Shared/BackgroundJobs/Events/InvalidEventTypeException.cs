namespace Expenso.TimeManagement.Core.Application.Jobs.Shared.BackgroundJobs.Events;

internal sealed class InvalidEventTypeException : Exception
{
    public InvalidEventTypeException(string type) : base(message: $"Invalid event type: {type}")
    {
    }
}
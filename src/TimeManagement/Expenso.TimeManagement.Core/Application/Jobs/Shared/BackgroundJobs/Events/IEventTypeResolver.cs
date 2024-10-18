namespace Expenso.TimeManagement.Core.Application.Jobs.Shared.BackgroundJobs.Events;

public interface IEventTypeResolver
{
    public bool IsAllowable(string eventName);

    public Type Resolve(string eventName);
}
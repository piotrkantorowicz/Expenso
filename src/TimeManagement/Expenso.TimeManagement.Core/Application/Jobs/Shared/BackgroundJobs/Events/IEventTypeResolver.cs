using Expenso.TimeManagement.Core.Application.Shared.Settings;

namespace Expenso.TimeManagement.Core.Application.Jobs.Shared.BackgroundJobs.Events;

public interface IEventTypeResolver
{
    public bool IsAllowable(AllowedEventType eventName);

    public Type Resolve(AllowedEventType eventName);
}
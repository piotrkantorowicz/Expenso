using Expenso.BudgetSharing.Shared.DTO.MessageBus.BudgetPermissionRequests;

namespace Expenso.TimeManagement.Core.Application.Jobs.Shared.BackgroundJobs.Events;

internal sealed class EventTypeResolver : IEventTypeResolver
{
    private static readonly Dictionary<string, Type> AllowedEventTypes = new()
    {
        { AllowedEvents.BudgetPermissionRequestExpired, typeof(BudgetPermissionRequestExpiredIntegrationEvent) }
    };

    public bool IsAllowable(string eventName)
    {
        return AllowedEventTypes.ContainsKey(key: eventName);
    }

    public Type Resolve(string eventName)
    {
        if (!AllowedEventTypes.TryGetValue(key: eventName, value: out Type? type))
        {
            throw new InvalidEventTypeException(type: eventName);
        }

        return type;
    }
}
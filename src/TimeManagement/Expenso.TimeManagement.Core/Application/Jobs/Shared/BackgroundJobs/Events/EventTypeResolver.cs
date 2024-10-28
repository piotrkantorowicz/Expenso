using Expenso.BudgetSharing.Shared.DTO.MessageBus.BudgetPermissionRequests;
using Expenso.TimeManagement.Core.Application.Shared.Settings;

using Microsoft.Extensions.Logging;

namespace Expenso.TimeManagement.Core.Application.Jobs.Shared.BackgroundJobs.Events;

internal sealed class EventTypeResolver : IEventTypeResolver
{
    private readonly Dictionary<AllowedEventType, Type> _allowedEventTypes = new();

    public EventTypeResolver(TimeManagementSettings timeManagementSettings, ILogger<EventTypeResolver> logger)
    {
        ArgumentNullException.ThrowIfNull(argument: logger);
        ArgumentNullException.ThrowIfNull(argument: timeManagementSettings);

        if (timeManagementSettings.AllowedEvents is null)
        {
            logger.LogWarning(message: "No allowed events are defined in the settings");

            return;
        }

        foreach (AllowedEventType allowedEventType in timeManagementSettings.AllowedEvents)
        {
            Type? type = allowedEventType switch
            {
                AllowedEventType.BudgetPermissionRequestExpired =>
                    typeof(BudgetPermissionRequestExpiredIntegrationEvent),
                AllowedEventType.None => null,
                _ => throw new InvalidEventTypeException(type: allowedEventType)
            };

            if (type is null)
            {
                logger.LogWarning(message: "No type is defined for event type {EventType}", allowedEventType);

                continue;
            }

            _allowedEventTypes.Add(key: allowedEventType, value: type);
        }
    }

    public bool IsAllowable(AllowedEventType eventName)
    {
        return _allowedEventTypes.ContainsKey(key: eventName);
    }

    public Type Resolve(AllowedEventType eventName)
    {
        if (!_allowedEventTypes.TryGetValue(key: eventName, value: out Type? type))
        {
            throw new InvalidEventTypeException(type: eventName);
        }

        return type;
    }
}
using Expenso.Communication.Core.Application.Notifications.Factories.Interfaces;
using Expenso.Communication.Core.Application.Notifications.Services;

namespace Expenso.Communication.Core.Application.Notifications.Factories;

internal sealed class NotificationServiceFactory(IDictionary<string, INotificationService> servicesDictionary)
    : INotificationServiceFactory
{
    private readonly IDictionary<string, INotificationService> _servicesDictionary =
        servicesDictionary ?? throw new ArgumentNullException(paramName: nameof(servicesDictionary));

    public T GetService<T>() where T : INotificationService
    {
        if (!_servicesDictionary.TryGetValue(key: typeof(T).Name, value: out INotificationService? service))
        {
            throw new InvalidOperationException(
                message: $"Notification service {typeof(T).FullName} hasn't been found.");
        }

        if (service is not T requestedService)
        {
            throw new InvalidOperationException(
                message: $"Notification service is not of requested type {typeof(T).FullName}.");
        }

        return requestedService;
    }
}
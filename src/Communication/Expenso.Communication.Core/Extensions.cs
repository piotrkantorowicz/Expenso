using System.Reflection;

using Expenso.Communication.Core.Application.Notifications.Factories;
using Expenso.Communication.Core.Application.Notifications.Factories.Interfaces;
using Expenso.Communication.Core.Application.Notifications.Services;

using Microsoft.Extensions.DependencyInjection;

namespace Expenso.Communication.Core;

public static class Extensions
{
    public static void AddCommunicationCore(this IServiceCollection services, IEnumerable<Assembly> assemblies)
    {
        services.Scan(action: selector =>
            selector
                .FromAssemblies(assemblies: assemblies)
                .AddClasses(action: c => c.AssignableTo(type: typeof(INotificationService)))
                .AsImplementedInterfaces()
                .WithScopedLifetime());

        services.AddScoped<INotificationServiceFactory>(implementationFactory: sp =>
        {
            IEnumerable<INotificationService> notificationServices =
                sp
                    .GetServices(serviceType: typeof(INotificationService))
                    .Select(selector: x => (INotificationService)x!);

            Dictionary<string, INotificationService> servicesDictionary =
                notificationServices.ToDictionary(
                    keySelector: service => service
                        .GetType()
                        .GetInterfaces()
                        .FirstOrDefault(predicate: x => x != typeof(INotificationService)) !.Name,
                    elementSelector: service => service);

            return new NotificationServiceFactory(servicesDictionary: servicesDictionary);
        });
    }
}
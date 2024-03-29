﻿using System.Reflection;

using Expenso.Communication.Core.Application.Notifications.Factories;
using Expenso.Communication.Core.Application.Notifications.Factories.Interfaces;
using Expenso.Communication.Core.Application.Notifications.Services;

using Microsoft.Extensions.DependencyInjection;

namespace Expenso.Communication.Core;

public static class Extensions
{
    public static void AddCommunicationCore(this IServiceCollection services, IEnumerable<Assembly> assemblies)
    {
        services.Scan(selector =>
            selector
                .FromAssemblies(assemblies)
                .AddClasses(c => c.AssignableTo(typeof(INotificationService)))
                .AsImplementedInterfaces()
                .WithScopedLifetime());

        services.AddScoped<INotificationServiceFactory>(sp =>
        {
            IEnumerable<INotificationService> notificationServices =
                sp.GetServices(typeof(INotificationService)).Select(x => (INotificationService)x!);

            Dictionary<string, INotificationService> servicesDictionary =
                notificationServices.ToDictionary(
                    service => service
                        .GetType()
                        .GetInterfaces()
                        .FirstOrDefault(x => x != typeof(INotificationService)) !.Name, service => service);

            return new NotificationServiceFactory(servicesDictionary);
        });
    }
}
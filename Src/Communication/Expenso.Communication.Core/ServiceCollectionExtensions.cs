using Expenso.Communication.Core.Application.Notifications.Factories;
using Expenso.Communication.Core.Application.Notifications.Factories.Interfaces;
using Expenso.Communication.Core.Application.Notifications.Services;
using Expenso.Communication.Core.Application.Notifications.Services.Emails.Acl.Fake;
using Expenso.Communication.Core.Application.Notifications.Services.InApp.Acl.Fake;
using Expenso.Communication.Core.Application.Notifications.Services.Push.Acl.Fake;

using Microsoft.Extensions.DependencyInjection;

namespace Expenso.Communication.Core;

public static class ServiceCollectionExtensions
{
    public static void AddCommunicationCore(this IServiceCollection services)
    {
        services.AddScoped<INotificationService, FakeEmailService>();
        services.AddScoped<INotificationService, FakePushService>();
        services.AddScoped<INotificationService, FakeInAppService>();

        services.AddScoped<INotificationServiceFactory>(sp =>
        {
            var notificationServices =
                sp.GetServices(typeof(INotificationService)).Select(x => (INotificationService)x!);

            var servicesDictionary =
                notificationServices.ToDictionary(service => service.GetType().Name, service => service);

            return new NotificationServiceFactory(servicesDictionary);
        });
    }
}
using Expenso.Communication.Core.Application.Notifications.Services;

namespace Expenso.Communication.Core.Application.Notifications.Factories.Interfaces;

internal interface INotificationServiceFactory
{
    T GetService<T>() where T : INotificationService;
}
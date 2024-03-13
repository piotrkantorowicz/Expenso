using Expenso.Communication.Core.Application.Notifications.Factories.Interfaces;
using Expenso.Communication.Core.Application.Notifications.Services.Emails;
using Expenso.Communication.Core.Application.Notifications.Services.InApp;
using Expenso.Communication.Core.Application.Notifications.Services.Push;
using Expenso.Communication.Proxy.DTO.API.SendNotification;
using Expenso.Shared.Commands;

namespace Expenso.Communication.Core.Application.Notifications.Write.Commands.SendNotification;

internal sealed class SendNotificationCommandHandler(INotificationServiceFactory notificationServiceFactory)
    : ICommandHandler<SendNotificationCommand>
{
    private readonly INotificationServiceFactory _notificationServiceFactory = notificationServiceFactory ??
                                                                               throw new ArgumentNullException(
                                                                                   nameof(notificationServiceFactory));

    public async Task HandleAsync(SendNotificationCommand command, CancellationToken cancellationToken)
    {
        (_, SendNotificationRequest? request) = command;

        (string? subject, string content, SendNotificationRequest_NotificationContext? context,
            SendNotificationRequest_NotificationType? type) = request!;

        (string from, string to, string[]? cc, string[]? bcc, string? replyTo) = context!;

        if (type?.Email == true)
        {
            IEmailService emailService = _notificationServiceFactory.GetService<IEmailService>();
            await emailService.SendNotificationAsync(from, to, subject, content, cc, bcc, replyTo);
        }

        if (type?.Push == true)
        {
            IPushService pushService = _notificationServiceFactory.GetService<IPushService>();
            await pushService.SendNotificationAsync(from, to, subject, content, cc, bcc, replyTo);
        }

        if (type?.InApp == true)
        {
            IInAppService inAppService = _notificationServiceFactory.GetService<IInAppService>();
            await inAppService.SendNotificationAsync(from, to, subject, content, cc, bcc, replyTo);
        }
    }
}
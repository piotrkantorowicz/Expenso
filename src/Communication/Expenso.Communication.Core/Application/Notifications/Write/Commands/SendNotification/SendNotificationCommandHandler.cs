using Expenso.Communication.Core.Application.Notifications.Factories.Interfaces;
using Expenso.Communication.Core.Application.Notifications.Services.Emails;
using Expenso.Communication.Core.Application.Notifications.Services.InApp;
using Expenso.Communication.Core.Application.Notifications.Services.Push;
using Expenso.Communication.Proxy.DTO.API.SendNotification;
using Expenso.Shared.Commands;
using Expenso.Shared.System.Types.Messages.Interfaces;

namespace Expenso.Communication.Core.Application.Notifications.Write.Commands.SendNotification;

internal sealed class SendNotificationCommandHandler : ICommandHandler<SendNotificationCommand>
{
    private readonly INotificationServiceFactory _notificationServiceFactory;

    public SendNotificationCommandHandler(INotificationServiceFactory notificationServiceFactory)
    {
        _notificationServiceFactory = notificationServiceFactory ??
                                      throw new ArgumentNullException(paramName: nameof(notificationServiceFactory));
    }

    public async Task HandleAsync(SendNotificationCommand command, CancellationToken cancellationToken)
    {
        (IMessageContext messageContext, SendNotificationRequest? request) = command;

        (string? subject, string content, SendNotificationRequest_NotificationContext? context,
            SendNotificationRequest_NotificationType? type) = request!;

        (string from, string to, string[]? cc, string[]? bcc, string? replyTo) = context!;

        if (type?.Email is true)
        {
            IEmailService emailService = _notificationServiceFactory.GetService<IEmailService>();

            await emailService.SendNotificationAsync(messageContext: messageContext, from: from, to: to,
                subject: subject, content: content, cc: cc, bcc: bcc, replyTo: replyTo);
        }

        if (type?.Push is true)
        {
            IPushService pushService = _notificationServiceFactory.GetService<IPushService>();

            await pushService.SendNotificationAsync(messageContext: messageContext, from: from, to: to,
                subject: subject, content: content, cc: cc, bcc: bcc, replyTo: replyTo);
        }

        if (type?.InApp is true)
        {
            IInAppService inAppService = _notificationServiceFactory.GetService<IInAppService>();

            await inAppService.SendNotificationAsync(messageContext: messageContext, from: from, to: to,
                subject: subject, content: content, cc: cc, bcc: bcc, replyTo: replyTo);
        }
    }
}
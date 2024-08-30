using Expenso.Shared.System.Types.Messages.Interfaces;

namespace Expenso.Communication.Core.Application.Notifications.Services;

internal interface INotificationService
{
    public Task SendNotificationAsync(IMessageContext messageContext, string from, string to, string? subject,
        string content, string[]? cc = null, string[]? bcc = null, string? replyTo = null);
}
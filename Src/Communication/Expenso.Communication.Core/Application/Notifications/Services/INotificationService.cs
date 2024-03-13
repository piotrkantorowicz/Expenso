namespace Expenso.Communication.Core.Application.Notifications.Services;

internal interface INotificationService
{
    public Task SendNotificationAsync(string from, string to, string? subject, string content, string[]? cc = null,
        string[]? bcc = null, string? replyTo = null);
}
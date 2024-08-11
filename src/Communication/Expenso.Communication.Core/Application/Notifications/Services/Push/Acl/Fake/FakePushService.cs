using Microsoft.Extensions.Logging;

namespace Expenso.Communication.Core.Application.Notifications.Services.Push.Acl.Fake;

internal sealed class FakePushService(ILogger<FakePushService> logger) : IPushService
{
    private readonly ILogger<FakePushService> _logger =
        logger ?? throw new ArgumentNullException(paramName: nameof(logger));

    public Task SendNotificationAsync(string from, string to, string? subject, string content, string[]? cc = null,
        string[]? bcc = null, string? replyTo = null)
    {
        _logger.LogInformation(
            message:
            "Push notification from {From} to {To} with subject {Subject} and content {Content} sent successfully",
            from, to, subject, content);

        return Task.CompletedTask;
    }
}
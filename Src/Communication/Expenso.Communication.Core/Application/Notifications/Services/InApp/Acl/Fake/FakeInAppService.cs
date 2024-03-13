using Microsoft.Extensions.Logging;

namespace Expenso.Communication.Core.Application.Notifications.Services.InApp.Acl.Fake;

internal sealed class FakeInAppService(ILogger<FakeInAppService> logger) : IInAppService
{
    private readonly ILogger<FakeInAppService> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    public Task SendNotificationAsync(string from, string to, string? subject, string content, string[]? cc = null,
        string[]? bcc = null, string? replyTo = null)
    {
        _logger.LogInformation(
            "App notification from {From} to {To} with subject {Subject} and content {Content} sent successfully", from,
            to, subject, content);

        return Task.CompletedTask;
    }
}
using Microsoft.Extensions.Logging;

namespace Expenso.Communication.Core.Application.Notifications.Services.InApp.Acl.Fake;

internal sealed class FakeInAppService : IInAppService
{
    private readonly ILogger<FakeInAppService> _logger;

    public FakeInAppService(ILogger<FakeInAppService> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(paramName: nameof(logger));
    }

    public Task SendNotificationAsync(string from, string to, string? subject, string content, string[]? cc = null,
        string[]? bcc = null, string? replyTo = null)
    {
        _logger.LogInformation(
            message:
            "App notification from {From} to {To} with subject {Subject} and content {Content} sent successfully", from,
            to, subject, content);

        return Task.CompletedTask;
    }
}
using Expenso.Communication.Core.Application.Notifications.Services.Push.Acl.Fake;

using Microsoft.Extensions.Logging;

namespace Expenso.Communication.Core.Application.Notifications.Services.Emails.Acl.Fake;

internal sealed class FakeEmailService(ILogger<FakeEmailService> logger) : IEmailService
{
    private readonly ILogger<FakeEmailService> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    public Task SendNotificationAsync(string from, string to, string? subject, string content, string[]? cc = null,
        string[]? bcc = null, string? replyTo = null)
    {
        _logger.LogInformation(
            "Email notification from {From} to {To} with cc {Cc} and bcc {Bcc} and replyTo {ReplyTo} with subject {Subject} and content {Content} sent successfully",
            from, to, cc is null ? null : string.Join(",", cc), bcc is null ? null : string.Join(",", bcc), replyTo,
            subject, content);

        return Task.CompletedTask;
    }
}
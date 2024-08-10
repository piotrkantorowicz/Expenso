using Microsoft.Extensions.Logging;

namespace Expenso.Communication.Core.Application.Notifications.Services.Emails.Acl.Fake;

internal sealed class FakeEmailService(ILogger<FakeEmailService> logger) : IEmailService
{
    private readonly ILogger<FakeEmailService> _logger =
        logger ?? throw new ArgumentNullException(paramName: nameof(logger));

    public Task SendNotificationAsync(string from, string to, string? subject, string content, string[]? cc = null,
        string[]? bcc = null, string? replyTo = null)
    {
        _logger.LogInformation(
            message:
            "Email notification from {From} to {To} with cc {Cc} and bcc {Bcc} and replyTo {ReplyTo} with subject {Subject} and content {Content} sent successfully",
            from, to, cc is null ? null : string.Join(separator: ",", value: cc),
            bcc is null ? null : string.Join(separator: ",", value: bcc), replyTo, subject, content);

        return Task.CompletedTask;
    }
}
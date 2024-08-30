using Expenso.Shared.System.Logging;
using Expenso.Shared.System.Types.Messages.Interfaces;

namespace Expenso.Communication.Core.Application.Notifications.Services.Emails.Acl.Fake;

internal sealed class FakeEmailService : IEmailService
{
    private readonly ILoggerService<FakeEmailService> _logger;

    public FakeEmailService(ILoggerService<FakeEmailService> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(paramName: nameof(logger));
    }

    public Task SendNotificationAsync(IMessageContext messageContext, string from, string to, string? subject,
        string content, string[]? cc = null, string[]? bcc = null, string? replyTo = null)
    {
        _logger.LogInfo(eventId: LoggingUtils.GeneralInformation,
            message:
            "Email notification from {From} to {To} with cc {Cc} and bcc {Bcc} and replyTo {ReplyTo} with subject {Subject} and content {Content} sent successfully",
            messageContext: messageContext, from, to, cc is null ? null : string.Join(separator: ",", value: cc),
            bcc is null ? null : string.Join(separator: ",", value: bcc), replyTo, subject, content);

        return Task.CompletedTask;
    }
}
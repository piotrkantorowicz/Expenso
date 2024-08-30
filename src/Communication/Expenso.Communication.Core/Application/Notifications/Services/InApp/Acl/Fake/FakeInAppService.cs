using Expenso.Shared.System.Logging;
using Expenso.Shared.System.Types.Messages.Interfaces;

namespace Expenso.Communication.Core.Application.Notifications.Services.InApp.Acl.Fake;

internal sealed class FakeInAppService : IInAppService
{
    private readonly ILoggerService<FakeInAppService> _logger;

    public FakeInAppService(ILoggerService<FakeInAppService> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(paramName: nameof(logger));
    }

    public Task SendNotificationAsync(IMessageContext messageContext, string from, string to, string? subject,
        string content, string[]? cc = null, string[]? bcc = null, string? replyTo = null)
    {
        _logger.LogInfo(eventId: LoggingUtils.GeneralInformation,
            message:
            "App notification from {From} to {To} with subject {Subject} and content {Content} sent successfully",
            messageContext: messageContext, from, to, subject, content);

        return Task.CompletedTask;
    }
}
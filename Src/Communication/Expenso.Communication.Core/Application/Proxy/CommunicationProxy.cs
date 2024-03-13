using Expenso.Communication.Core.Application.Notifications.Write.Commands.SendNotification;
using Expenso.Communication.Proxy;
using Expenso.Communication.Proxy.DTO.API.SendNotification;
using Expenso.Shared.Commands.Dispatchers;
using Expenso.Shared.System.Types.Messages.Interfaces;

namespace Expenso.Communication.Core.Application.Proxy;

internal sealed class CommunicationProxy(
    ICommandDispatcher commandDispatcher,
    IMessageContextFactory messageContextFactory) : ICommunicationProxy
{
    private readonly ICommandDispatcher _commandDispatcher =
        commandDispatcher ?? throw new ArgumentNullException(nameof(commandDispatcher));

    private readonly IMessageContextFactory _messageContextFactory =
        messageContextFactory ?? throw new ArgumentNullException(nameof(messageContextFactory));

    public async Task SendNotificationAsync(SendNotificationRequest request,
        CancellationToken cancellationToken = default)
    {
        await _commandDispatcher.SendAsync(new SendNotificationCommand(_messageContextFactory.Current(), request),
            cancellationToken);
    }
}
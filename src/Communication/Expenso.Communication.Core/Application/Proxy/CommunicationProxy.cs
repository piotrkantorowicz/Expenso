using Expenso.Communication.Core.Application.Notifications.Write.Commands.SendNotification;
using Expenso.Communication.Shared;
using Expenso.Communication.Shared.DTO.API.SendNotification;
using Expenso.Shared.Commands.Dispatchers;
using Expenso.Shared.System.Types.Messages.Interfaces;

namespace Expenso.Communication.Core.Application.Proxy;

internal sealed class CommunicationProxy : ICommunicationProxy
{
    private readonly ICommandDispatcher _commandDispatcher;
    private readonly IMessageContextFactory _messageContextFactory;

    public CommunicationProxy(ICommandDispatcher commandDispatcher, IMessageContextFactory messageContextFactory)
    {
        _commandDispatcher = commandDispatcher ?? throw new ArgumentNullException(paramName: nameof(commandDispatcher));

        _messageContextFactory = messageContextFactory ??
                                 throw new ArgumentNullException(paramName: nameof(messageContextFactory));
    }

    public async Task SendNotificationAsync(SendNotificationRequest request,
        CancellationToken cancellationToken = default)
    {
        await _commandDispatcher.SendAsync(
            command: new SendNotificationCommand(MessageContext: _messageContextFactory.Current(),
                SendNotificationRequest: request), cancellationToken: cancellationToken);
    }

    public async Task SendNotificationsAsync(IReadOnlyCollection<SendNotificationRequest> requests,
        CancellationToken cancellationToken = default)
    {
        List<Task> sendNotificationTasks = [];

        foreach (SendNotificationRequest request in requests)
        {
            sendNotificationTasks.Add(item: _commandDispatcher.SendAsync(
                command: new SendNotificationCommand(MessageContext: _messageContextFactory.Current(),
                    SendNotificationRequest: request), cancellationToken: cancellationToken));
        }

        await Task.WhenAll(tasks: sendNotificationTasks);
    }
}
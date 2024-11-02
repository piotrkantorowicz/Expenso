using Expenso.Communication.Shared.DTO.API.SendNotification;
using Expenso.Shared.System.Types.Messages.Interfaces;

namespace Expenso.Communication.Shared;

public interface ICommunicationProxy
{
    public Task SendNotificationAsync(SendNotificationRequest request, IMessageContext? messageContext = null,
        CancellationToken cancellationToken = default);

    public Task SendNotificationsAsync(IReadOnlyCollection<SendNotificationRequest> requests,
        IMessageContext? messageContext = null, CancellationToken cancellationToken = default);
}
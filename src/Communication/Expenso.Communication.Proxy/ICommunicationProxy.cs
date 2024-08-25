using Expenso.Communication.Proxy.DTO.API.SendNotification;

namespace Expenso.Communication.Proxy;

public interface ICommunicationProxy
{
    public Task SendNotificationAsync(SendNotificationRequest request, CancellationToken cancellationToken = default);

    public Task SendNotificationsAsync(IReadOnlyCollection<SendNotificationRequest> requests,
        CancellationToken cancellationToken = default);
}
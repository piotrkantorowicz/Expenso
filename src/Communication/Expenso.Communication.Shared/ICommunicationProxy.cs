using Expenso.Communication.Shared.DTO.API.SendNotification;

namespace Expenso.Communication.Shared;

public interface ICommunicationProxy
{
    public Task SendNotificationAsync(SendNotificationRequest request, CancellationToken cancellationToken = default);

    public Task SendNotificationsAsync(IReadOnlyCollection<SendNotificationRequest> requests,
        CancellationToken cancellationToken = default);
}
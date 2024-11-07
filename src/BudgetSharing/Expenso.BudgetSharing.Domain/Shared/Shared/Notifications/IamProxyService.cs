using Expenso.BudgetSharing.Domain.Shared.Shared.Notifications.Models;
using Expenso.BudgetSharing.Domain.Shared.ValueObjects;
using Expenso.IAM.Shared;
using Expenso.IAM.Shared.DTO.GetUsers.Request;
using Expenso.IAM.Shared.DTO.GetUsers.Response;
using Expenso.Shared.System.Logging;
using Expenso.Shared.System.Types.Messages.Interfaces;

namespace Expenso.BudgetSharing.Domain.Shared.Shared.Notifications;

internal sealed class IamProxyService : IIamProxyService
{
    private readonly IIamProxy _iamProxy;
    private readonly ILoggerService<IamProxyService> _logger;

    public IamProxyService(IIamProxy iamProxy, ILoggerService<IamProxyService> logger)
    {
        _iamProxy = iamProxy ?? throw new ArgumentNullException(paramName: nameof(iamProxy));
        _logger = logger ?? throw new ArgumentNullException(paramName: nameof(logger));
    }

    // TODO: Consider using user preferences to determine if a user should receive notifications
    public async Task<NotificationRecipients> GetUserNotificationAvailability(IMessageContext messageContext,
        PersonId ownerId, IReadOnlyCollection<PersonId> participantIds, CancellationToken cancellationToken)
    {
        // As long as keycloak not support get many users by ids, we will get all users and filter them
        // it is not optimal, but we don't have so many users in the system so it is fine for now
        // there is a feature request to add get many users by ids in keycloak
        // https://github.com/keycloak/keycloak/issues/12025
        IReadOnlyCollection<GetUsersResponse> users = await _iamProxy.GetUsersAsync(
            request: new GetUsersRequest(Limit: int.MaxValue), cancellationToken: cancellationToken) ?? [];

        ICollection<NotificationRecipient> participantsNotificationModels = users
            .Where(predicate: x => participantIds.Select(selector: y => y.ToString()).Contains(value: x.UserId))
            .Select(selector: x => new NotificationRecipient(UserId: x.UserId, Email: x.Email, Fullname: x.Fullname))
            .ToList();

        GetUsersResponse? owner = users.FirstOrDefault(predicate: x => x.UserId == ownerId.ToString());

        if (owner is null)
        {
            _logger.LogWarning(eventId: LoggingUtils.GeneralWarning,
                message:
                "Cannot send notification to owner '{OwnerId}' as their notification details could not be found",
                messageContext: messageContext, args: ownerId);
        }

        return new NotificationRecipients(
            Owner: owner is null
                ? NotificationRecipient.Empty
                : new NotificationRecipient(UserId: owner.UserId, Email: owner.Email, Fullname: owner.Fullname),
            Participants: participantsNotificationModels.ToList().AsReadOnly());
    }
}
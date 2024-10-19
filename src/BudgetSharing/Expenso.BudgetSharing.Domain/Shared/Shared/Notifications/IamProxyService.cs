using Expenso.BudgetSharing.Domain.Shared.Shared.Notifications.Models;
using Expenso.BudgetSharing.Domain.Shared.ValueObjects;
using Expenso.IAM.Shared;
using Expenso.IAM.Shared.DTO.GetUser.Response;
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

    public async Task<UserNotificationModel> GetUserNotificationAvailability(IMessageContext messageContext,
        PersonId ownerId, IReadOnlyCollection<PersonId> participantIds, CancellationToken cancellationToken)
    {
        GetUserResponse? owner =
            await _iamProxy.GetUserByIdAsync(userId: ownerId.ToString(), cancellationToken: cancellationToken);

        bool canSendNotificationToOwner = true;

        if (owner is null)
        {
            _logger.LogWarning(eventId: LoggingUtils.GeneralWarning,
                message: "Cannot send notification to owner '{OwnerId}' as their email could not be found",
                messageContext: messageContext, args: ownerId);

            canSendNotificationToOwner = false;
        }

        ICollection<PersonNotificationModel> participantsNotificationModels = [];

        foreach (PersonId participantId in participantIds)
        {
            GetUserResponse? participant = await _iamProxy.GetUserByIdAsync(userId: participantId.ToString(),
                cancellationToken: cancellationToken);

            bool canSendNotificationToParticipant = true;

            if (participant is null)
            {
                _logger.LogWarning(eventId: LoggingUtils.GeneralWarning,
                    message:
                    "Cannot send notification to participant {ParticipantId} as their email could not be found",
                    messageContext: messageContext, args: participantId);

                canSendNotificationToParticipant = false;
            }

            participantsNotificationModels.Add(item: new PersonNotificationModel(Person: participant,
                CanSendNotification: canSendNotificationToParticipant));
        }

        return new UserNotificationModel(
            Owner: new PersonNotificationModel(Person: owner, CanSendNotification: canSendNotificationToOwner),
            Participants: participantsNotificationModels.ToList().AsReadOnly());
    }
}
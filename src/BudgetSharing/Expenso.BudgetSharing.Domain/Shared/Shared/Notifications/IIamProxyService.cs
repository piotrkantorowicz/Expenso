using Expenso.BudgetSharing.Domain.Shared.Shared.Notifications.Models;
using Expenso.BudgetSharing.Domain.Shared.ValueObjects;
using Expenso.Shared.System.Types.Messages.Interfaces;

namespace Expenso.BudgetSharing.Domain.Shared.Shared.Notifications;

public interface IIamProxyService
{
    Task<UserNotificationModel> GetUserNotificationAvailability(IMessageContext messageContext, PersonId ownerId,
        IReadOnlyCollection<PersonId> participantIds, CancellationToken cancellationToken);
}
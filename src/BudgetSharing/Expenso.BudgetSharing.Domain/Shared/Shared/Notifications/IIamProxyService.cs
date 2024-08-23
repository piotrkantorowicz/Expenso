using Expenso.BudgetSharing.Domain.Shared.Shared.Notifications.Models;
using Expenso.BudgetSharing.Domain.Shared.ValueObjects;

namespace Expenso.BudgetSharing.Domain.Shared.Shared.Notifications;

public interface IIamProxyService
{
    Task<UserNotificationModel> GetUserNotificationAvailability(PersonId ownerId,
        IReadOnlyCollection<PersonId> participantIds, CancellationToken cancellationToken);
}
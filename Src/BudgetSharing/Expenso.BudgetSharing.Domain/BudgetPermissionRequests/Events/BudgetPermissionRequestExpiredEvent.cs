using Expenso.BudgetSharing.Domain.Shared.Model.ValueObjects;
using Expenso.Shared.Domain.Types.Events;
using Expenso.Shared.Domain.Types.ValueObjects;

namespace Expenso.BudgetSharing.Domain.BudgetPermissionRequests.Events;

internal sealed record BudgetPermissionRequestExpiredEvent(
    BudgetId BudgetId,
    PersonId ParticipantId,
    PermissionType PermissionType,
    DateAndTime? ExpiryDate) : IDomainEvent;
using Expenso.BudgetSharing.Domain.Shared.Model.ValueObjects;
using Expenso.Shared.Domain.Types.Events;

namespace Expenso.BudgetSharing.Domain.BudgetPermissionRequests.Events;

internal sealed record BudgetPermissionRequestedEvent(
    BudgetId BudgetId,
    PersonId ParticipantId,
    PermissionType PermissionType) : IDomainEvent;
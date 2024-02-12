using Expenso.BudgetSharing.Domain.Shared.Model.ValueObjects;
using Expenso.Shared.Domain.Types.Events;

namespace Expenso.BudgetSharing.Domain.BudgetPermissionRequests.Events;

internal sealed record BudgetPermissionRequestConfirmedEvent(
    BudgetId BudgetId,
    PersonId ParticipantId,
    PermissionType PermissionType) : IDomainEvent;
using Expenso.BudgetSharing.Domain.BudgetPermissions.ValueObjects;
using Expenso.BudgetSharing.Domain.Shared.ValueObjects;
using Expenso.Shared.Domain.Types.Events;
using Expenso.Shared.System.Types.Messages.Interfaces;

namespace Expenso.BudgetSharing.Domain.BudgetPermissions.Events;

internal sealed record BudgetPermissionDeletedEvent(
    IMessageContext MessageContext,
    BudgetPermissionId BudgetPermissionId,
    BudgetId BudgetId,
    IReadOnlyCollection<PersonId> ParticipantIds) : IDomainEvent;
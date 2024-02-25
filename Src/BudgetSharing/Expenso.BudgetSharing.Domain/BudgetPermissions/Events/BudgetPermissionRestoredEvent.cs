using Expenso.BudgetSharing.Domain.BudgetPermissions.ValueObjects;
using Expenso.BudgetSharing.Domain.Shared.Model.ValueObjects;
using Expenso.Shared.Domain.Types.Events;
using Expenso.Shared.System.Types.Messages.Interfaces;

namespace Expenso.BudgetSharing.Domain.BudgetPermissions.Events;

internal sealed record BudgetPermissionRestoredEvent(
    IMessageContext MessageContext,
    BudgetPermissionId BudgetPermissionId,
    BudgetId BudgetId,
    IReadOnlyCollection<PersonId> ParticipantIds) : IDomainEvent;
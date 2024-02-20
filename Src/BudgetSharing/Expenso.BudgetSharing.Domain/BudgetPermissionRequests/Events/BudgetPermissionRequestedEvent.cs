using Expenso.BudgetSharing.Domain.Shared.Model.ValueObjects;
using Expenso.Shared.Domain.Types.Events;
using Expenso.Shared.System.Types.Messages.Interfaces;

namespace Expenso.BudgetSharing.Domain.BudgetPermissionRequests.Events;

internal sealed record BudgetPermissionRequestedEvent(
    IMessageContext MessageContext,
    BudgetId BudgetId,
    PersonId ParticipantId,
    PermissionType PermissionType) : IDomainEvent;
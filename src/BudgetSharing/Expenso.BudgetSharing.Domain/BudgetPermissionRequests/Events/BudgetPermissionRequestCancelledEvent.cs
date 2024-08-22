using Expenso.BudgetSharing.Domain.Shared.ValueObjects;
using Expenso.Shared.Domain.Types.Events;
using Expenso.Shared.System.Types.Messages.Interfaces;

namespace Expenso.BudgetSharing.Domain.BudgetPermissionRequests.Events;

internal sealed record BudgetPermissionRequestCancelledEvent(
    IMessageContext MessageContext,
    PersonId OwnerId,
    PersonId ParticipantId,
    PermissionType PermissionType) : IDomainEvent;
using Expenso.BudgetSharing.Domain.Shared.ValueObjects;
using Expenso.Shared.Domain.Types.Events;
using Expenso.Shared.Domain.Types.ValueObjects;
using Expenso.Shared.System.Types.Messages.Interfaces;

namespace Expenso.BudgetSharing.Domain.BudgetPermissionRequests.Events;

internal sealed record BudgetPermissionRequestExpiredEvent(
    IMessageContext MessageContext,
    PersonId OwnerId,
    PersonId ParticipantId,
    PermissionType PermissionType,
    DateAndTime? ExpirationDate) : IDomainEvent;
using Expenso.BudgetSharing.Domain.Shared.ValueObjects;
using Expenso.Shared.Domain.Types.Events;
using Expenso.Shared.System.Types.Messages.Interfaces;

namespace Expenso.BudgetSharing.Domain.BudgetPermissions.Events;

internal sealed record BudgetPermissionBlockedEvent(
    IMessageContext MessageContext,
    PersonId OwnerId,
    DateTimeOffset? BlockDate,
    IReadOnlyCollection<Permission> Permissions) : IDomainEvent;
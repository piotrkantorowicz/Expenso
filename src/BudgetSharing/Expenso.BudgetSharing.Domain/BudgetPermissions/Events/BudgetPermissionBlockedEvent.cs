using Expenso.BudgetSharing.Domain.Shared.ValueObjects;
using Expenso.Shared.Domain.Types.Events;
using Expenso.Shared.Domain.Types.ValueObjects;
using Expenso.Shared.System.Types.Messages.Interfaces;

namespace Expenso.BudgetSharing.Domain.BudgetPermissions.Events;

internal sealed record BudgetPermissionBlockedEvent(
    IMessageContext MessageContext,
    PersonId OwnerId,
    DateAndTime? BlockDate,
    IReadOnlyCollection<Permission> Permissions) : IDomainEvent;
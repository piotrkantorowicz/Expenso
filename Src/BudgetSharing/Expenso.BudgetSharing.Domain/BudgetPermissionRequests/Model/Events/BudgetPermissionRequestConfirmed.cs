using Expenso.BudgetSharing.Domain.BudgetPermissionRequests.Model.ValueObjects;
using Expenso.Shared.Domain.Types.Events;

namespace Expenso.BudgetSharing.Domain.BudgetPermissionRequests.Model.Events;

internal sealed record BudgetPermissionRequestConfirmed(BudgetPermissionRequestId BudgetPermissionRequestId)
    : IDomainEvent;

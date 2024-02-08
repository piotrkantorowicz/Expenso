using Expenso.BudgetSharing.Domain.BudgetPermissionRequests.ValueObjects;
using Expenso.BudgetSharing.Domain.Shared.Model.ValueObjects;

namespace Expenso.BudgetSharing.Application.Read.Shared.QueryStore.Filters;

public sealed record BudgetPermissionRequestFilter(
    Guid? Id = null,
    Guid? BudgetId = null,
    Guid? ParticipantId = null,
    BudgetPermissionRequestStatus? Status = null,
    PermissionType? PermissionType = null);
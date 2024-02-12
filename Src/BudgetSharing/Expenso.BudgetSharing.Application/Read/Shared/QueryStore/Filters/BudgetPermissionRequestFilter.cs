using Expenso.BudgetSharing.Domain.BudgetPermissionRequests.ValueObjects;
using Expenso.BudgetSharing.Domain.Shared.Model.ValueObjects;

namespace Expenso.BudgetSharing.Application.Read.Shared.QueryStore.Filters;

public sealed record BudgetPermissionRequestFilter(
    BudgetPermissionRequestId? Id = null,
    BudgetId? BudgetId = null,
    PersonId? ParticipantId = null,
    BudgetPermissionRequestStatus? Status = null,
    PermissionType? PermissionType = null);
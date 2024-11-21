using Expenso.BudgetSharing.Domain.BudgetPermissionRequests.ValueObjects;
using Expenso.BudgetSharing.Domain.Shared.ValueObjects;

namespace Expenso.BudgetSharing.Application.Shared.QueryStore.Filters;

public sealed record BudgetPermissionRequestFilter(
    BudgetPermissionRequestId? Id = null,
    BudgetId? BudgetId = null,
    BudgetCode? BudgetCode = null,
    PersonId? ParticipantId = null,
    PersonId? OwnerId = null,
    BudgetPermissionRequestStatus[]? Statuses = null,
    PermissionType[]? PermissionTypes = null);
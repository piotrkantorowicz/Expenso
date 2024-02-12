using Expenso.BudgetSharing.Domain.BudgetPermissionRequests.ValueObjects;
using Expenso.BudgetSharing.Domain.BudgetPermissions.ValueObjects;
using Expenso.BudgetSharing.Domain.Shared.Model.ValueObjects;

namespace Expenso.BudgetSharing.Application.Read.Shared.QueryStore.Filters;

public sealed record BudgetPermissionFilter(
    BudgetPermissionId? Id = null,
    BudgetId? BudgetId = null,
    PersonId? OwnerId = null,
    PersonId? ParticipantId = null,
    bool? IncludePermissions = null,
    PermissionType? PermissionType = null);
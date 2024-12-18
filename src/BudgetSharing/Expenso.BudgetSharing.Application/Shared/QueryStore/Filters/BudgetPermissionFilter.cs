using Expenso.BudgetSharing.Domain.BudgetPermissions.ValueObjects;
using Expenso.BudgetSharing.Domain.Shared.ValueObjects;
using Expenso.Shared.System.Types.Pagination;

namespace Expenso.BudgetSharing.Application.Shared.QueryStore.Filters;

public sealed record BudgetPermissionFilter(
    BudgetPermissionId? Id = null,
    BudgetId? BudgetId = null,
    BudgetCode? BudgetCode = null,
    PersonId? OwnerId = null,
    PersonId? ParticipantId = null,
    PermissionType[]? PermissionTypes = null,
    Paging? Pagination = null);
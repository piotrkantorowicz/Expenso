using Expenso.BudgetSharing.Domain.Shared.Model.ValueObjects;

namespace Expenso.BudgetSharing.Application.Read.Shared.QueryStore.Filters;

public sealed record BudgetPermissionFilter(
    Guid? Id = null,
    Guid? BudgetId = null,
    Guid? OwnerId = null,
    Guid? ParticipantId = null,
    bool? IncludePermissions = null,
    PermissionType? PermissionType = null);
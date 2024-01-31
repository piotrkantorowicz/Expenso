using Expenso.BudgetSharing.Domain.Shared.Model.ValueObjects;

namespace Expenso.BudgetSharing.Application.QueryStore.Filters;

public sealed record BudgetPermissionFilter
{
    public Guid? Id { get; init; }

    public Guid? BudgetId { get; init; }

    public Guid? ParticipantId { get; init; }

    public bool? IncludePermissions { get; init; }

    public PermissionType? PermissionType { get; init; }
}
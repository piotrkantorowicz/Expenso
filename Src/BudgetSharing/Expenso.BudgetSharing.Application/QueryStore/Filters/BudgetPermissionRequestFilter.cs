using Expenso.BudgetSharing.Domain.BudgetPermissionRequests.Model.ValueObjects;
using Expenso.BudgetSharing.Domain.Shared.Model.ValueObjects;

namespace Expenso.BudgetSharing.Application.QueryStore.Filters;

public sealed record BudgetPermissionRequestFilter
{
    public Guid? Id { get; init; }

    public Guid? BudgetId { get; init; }

    public Guid? ParticipantId { get; init; }

    public BudgetPermissionRequestStatus? Status { get; init; }

    public PermissionType? PermissionType { get; init; }
}
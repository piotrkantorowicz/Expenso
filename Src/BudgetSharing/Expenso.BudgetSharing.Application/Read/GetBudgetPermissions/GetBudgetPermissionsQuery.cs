using Expenso.BudgetSharing.Application.Read.GetBudgetPermissions.DTO.Requests;
using Expenso.BudgetSharing.Application.Read.GetBudgetPermissions.DTO.Responses;
using Expenso.Shared.Queries;

namespace Expenso.BudgetSharing.Application.Read.GetBudgetPermissions;

public sealed record GetBudgetPermissionsQuery(
    Guid? BudgetId = null,
    Guid? OwnerId = null,
    Guid? ParticipantId = null,
    bool? ForCurrentUser = null,
    bool? IncludePermissions = null,
    GetBudgetPermissionsRequestPermissionType? PermissionType = null)
    : IQuery<IReadOnlyCollection<GetBudgetPermissionsResponse>>;
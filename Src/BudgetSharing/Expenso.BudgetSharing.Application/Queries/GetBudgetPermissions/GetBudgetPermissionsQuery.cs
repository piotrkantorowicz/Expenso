using Expenso.BudgetSharing.Application.DTO.GetBudgetPermissions.Requests;
using Expenso.BudgetSharing.Application.DTO.GetBudgetPermissions.Responses;
using Expenso.Shared.Queries;

namespace Expenso.BudgetSharing.Application.Queries.GetBudgetPermissions;

public sealed record GetBudgetPermissionsQuery(
    Guid? Id = null,
    Guid? BudgetId = null,
    Guid? ParticipantId = null,
    bool? ForCurrentUser = null,
    bool? IncludePermissions = null,
    GetBudgetPermissionsRequestPermissionType? PermissionType = null)
    : IQuery<IReadOnlyCollection<GetBudgetPermissionsResponse>>;
using Expenso.BudgetSharing.Application.Read.GetBudgetPermission.DTO.Responses;
using Expenso.Shared.Queries;

namespace Expenso.BudgetSharing.Application.Read.GetBudgetPermission;

public sealed record GetBudgetPermissionQuery(Guid BudgetPermissionId, bool? IncludePermissions = null)
    : IQuery<GetBudgetPermissionResponse>;
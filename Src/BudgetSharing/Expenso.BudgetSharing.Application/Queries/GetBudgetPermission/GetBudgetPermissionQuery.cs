using Expenso.BudgetSharing.Application.DTO.GetBudgetPermission.Responses;
using Expenso.Shared.Queries;

namespace Expenso.BudgetSharing.Application.Queries.GetBudgetPermission;

public sealed record GetBudgetPermissionQuery(Guid Id, bool? IncludePermissions = null)
    : IQuery<GetBudgetPermissionResponse>;
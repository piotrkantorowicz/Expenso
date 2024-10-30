using Expenso.BudgetSharing.Application.BudgetPermissions.Read.GetBudgetPermission.DTO.Request;
using Expenso.BudgetSharing.Application.BudgetPermissions.Read.GetBudgetPermission.DTO.Response;
using Expenso.Shared.Queries;
using Expenso.Shared.System.Types.Messages.Interfaces;

namespace Expenso.BudgetSharing.Application.BudgetPermissions.Read.GetBudgetPermission;

public sealed record GetBudgetPermissionQuery(IMessageContext MessageContext, GetBudgetPermissionRequest? Payload)
    : IQuery<GetBudgetPermissionResponse>;
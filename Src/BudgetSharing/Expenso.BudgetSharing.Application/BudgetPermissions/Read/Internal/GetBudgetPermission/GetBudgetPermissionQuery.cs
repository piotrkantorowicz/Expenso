using Expenso.BudgetSharing.Application.BudgetPermissions.Read.Internal.GetBudgetPermission.DTO.Responses;
using Expenso.Shared.Queries;
using Expenso.Shared.System.Types.Messages.Interfaces;

namespace Expenso.BudgetSharing.Application.BudgetPermissions.Read.Internal.GetBudgetPermission;

public sealed record GetBudgetPermissionQuery(IMessageContext MessageContext, Guid BudgetPermissionId)
    : IQuery<GetBudgetPermissionResponse>;
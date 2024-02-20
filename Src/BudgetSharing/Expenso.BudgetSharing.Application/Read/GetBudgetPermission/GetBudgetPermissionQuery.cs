using Expenso.BudgetSharing.Application.Read.GetBudgetPermission.DTO.Responses;
using Expenso.Shared.Queries;
using Expenso.Shared.System.Types.Messages.Interfaces;

namespace Expenso.BudgetSharing.Application.Read.GetBudgetPermission;

public sealed record GetBudgetPermissionQuery(
    IMessageContext MessageContext,
    Guid BudgetPermissionId,
    bool? IncludePermissions = null) : IQuery<GetBudgetPermissionResponse>;
using Expenso.BudgetSharing.Application.BudgetPermissionRequests.Read.GetBudgetPermissionRequest.DTO.Response;
using Expenso.Shared.Queries;
using Expenso.Shared.System.Types.Messages.Interfaces;

namespace Expenso.BudgetSharing.Application.BudgetPermissionRequests.Read.GetBudgetPermissionRequest;

public sealed record GetBudgetPermissionRequestQuery(IMessageContext MessageContext, Guid BudgetPermissionRequestId)
    : IQuery<GetBudgetPermissionRequestResponse>;
using Expenso.BudgetSharing.Application.Read.GetBudgetPermissionRequest.DTO.Responses;
using Expenso.Shared.Queries;
using Expenso.Shared.System.Types.Messages.Interfaces;

namespace Expenso.BudgetSharing.Application.Read.GetBudgetPermissionRequest;

public sealed record GetBudgetPermissionRequestQuery(IMessageContext MessageContext, Guid BudgetPermissionRequestId)
    : IQuery<GetBudgetPermissionRequestResponse>;
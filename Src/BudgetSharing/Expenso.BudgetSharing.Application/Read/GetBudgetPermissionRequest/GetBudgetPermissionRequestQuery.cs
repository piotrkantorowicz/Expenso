using Expenso.BudgetSharing.Application.Read.GetBudgetPermissionRequest.DTO.Responses;
using Expenso.Shared.Queries;

namespace Expenso.BudgetSharing.Application.Read.GetBudgetPermissionRequest;

public sealed record GetBudgetPermissionRequestQuery(Guid BudgetPermissionRequestId)
    : IQuery<GetBudgetPermissionRequestResponse>;
using Expenso.BudgetSharing.Application.BudgetPermissionRequests.Read.GetBudgetPermissionRequests.DTO.Request;
using Expenso.BudgetSharing.Application.BudgetPermissionRequests.Read.GetBudgetPermissionRequests.DTO.Response;
using Expenso.Shared.Queries;
using Expenso.Shared.System.Types.Messages.Interfaces;

namespace Expenso.BudgetSharing.Application.BudgetPermissionRequests.Read.GetBudgetPermissionRequests;

public sealed record GetBudgetPermissionRequestsQuery(
    IMessageContext MessageContext,
    GetBudgetPermissionRequestsRequest? Payload) : IQuery<IReadOnlyCollection<GetBudgetPermissionRequestsResponse>>;
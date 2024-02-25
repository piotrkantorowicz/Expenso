using Expenso.BudgetSharing.Application.BudgetPermissionRequests.Read.GetBudgetPermissionRequests.DTO.Requests;
using Expenso.BudgetSharing.Application.BudgetPermissionRequests.Read.GetBudgetPermissionRequests.DTO.Responses;
using Expenso.Shared.Queries;
using Expenso.Shared.System.Types.Messages.Interfaces;

namespace Expenso.BudgetSharing.Application.BudgetPermissionRequests.Read.GetBudgetPermissionRequests;

public sealed record GetBudgetPermissionRequestsQuery(
    IMessageContext MessageContext,
    Guid? BudgetId = null,
    Guid? ParticipantId = null,
    bool? ForCurrentUser = null,
    GetBudgetPermissionRequestsRequestStatus? Status = null,
    GetBudgetPermissionRequestsRequestPermissionType? PermissionType = null)
    : IQuery<IReadOnlyCollection<GetBudgetPermissionRequestsResponse>>;
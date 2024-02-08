using Expenso.BudgetSharing.Application.Read.GetBudgetPermissionRequests.DTO.Requests;
using Expenso.BudgetSharing.Application.Read.GetBudgetPermissionRequests.DTO.Responses;
using Expenso.Shared.Queries;

namespace Expenso.BudgetSharing.Application.Read.GetBudgetPermissionRequests;

public sealed record GetBudgetPermissionRequestsQuery(
    Guid? BudgetId = null,
    Guid? ParticipantId = null,
    bool? ForCurrentUser = null,
    GetBudgetPermissionRequestsRequestStatus? Status = null,
    GetBudgetPermissionRequestsRequestPermissionType? PermissionType = null)
    : IQuery<IReadOnlyCollection<GetBudgetPermissionRequestsResponse>>;
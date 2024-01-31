using Expenso.BudgetSharing.Application.DTO.GetBudgetPermissionRequests.Requests;
using Expenso.BudgetSharing.Application.DTO.GetBudgetPermissionRequests.Responses;
using Expenso.Shared.Queries;

namespace Expenso.BudgetSharing.Application.Queries.GetBudgetPermissionRequests;

public sealed record GetBudgetPermissionRequestsQuery(
    Guid? BudgetId = null,
    Guid? ParticipantId = null,
    bool? ForCurrentUser = null,
    GetBudgetPermissionRequestsRequestStatus? Status = null,
    GetBudgetPermissionRequestsRequestPermissionType? PermissionType = null)
    : IQuery<IReadOnlyCollection<GetBudgetPermissionRequestsResponse>>;
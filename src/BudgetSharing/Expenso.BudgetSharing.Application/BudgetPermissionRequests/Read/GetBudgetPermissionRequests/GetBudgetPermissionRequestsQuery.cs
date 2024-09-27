using Expenso.BudgetSharing.Application.BudgetPermissionRequests.Read.GetBudgetPermissionRequests.DTO.Request;
using Expenso.BudgetSharing.Application.BudgetPermissionRequests.Read.GetBudgetPermissionRequests.DTO.Response;
using Expenso.Shared.Queries;
using Expenso.Shared.System.Types.Messages.Interfaces;

namespace Expenso.BudgetSharing.Application.BudgetPermissionRequests.Read.GetBudgetPermissionRequests;

public sealed record GetBudgetPermissionRequestsQuery(
    IMessageContext MessageContext,
    Guid? BudgetId = null,
    Guid? ParticipantId = null,
    Guid? OwnerId = null,
    bool? ForCurrentUser = null,
    GetBudgetPermissionRequestsRequest_Status? Status = null,
    GetBudgetPermissionRequestsRequest_PermissionType? PermissionType = null)
    : IQuery<IReadOnlyCollection<GetBudgetPermissionRequestsResponse>>;
using Expenso.BudgetSharing.Application.DTO.GetBudgetPermissionRequests.Requests;
using Expenso.BudgetSharing.Application.DTO.GetBudgetPermissionRequests.Requests.Maps;
using Expenso.BudgetSharing.Application.DTO.GetBudgetPermissionRequests.Responses;
using Expenso.BudgetSharing.Application.DTO.GetBudgetPermissionRequests.Responses.Maps;
using Expenso.BudgetSharing.Application.QueryStore.Filters;
using Expenso.BudgetSharing.Application.QueryStore.Repositories;
using Expenso.BudgetSharing.Domain.BudgetPermissionRequests.Model;
using Expenso.Shared.Queries;
using Expenso.Shared.UserContext;

namespace Expenso.BudgetSharing.Application.Queries.GetBudgetPermissionRequests;

internal sealed class GetBudgetPermissionRequestsQueryHandler(
    IBudgetPermissionRequestReadRepository budgetPermissionRequestRepository,
    IUserContextAccessor userContextAccessor)
    : IQueryHandler<GetBudgetPermissionRequestsQuery, IReadOnlyCollection<GetBudgetPermissionRequestsResponse>>
{
    private readonly IBudgetPermissionRequestReadRepository _budgetPermissionRequestRepository =
        budgetPermissionRequestRepository ?? throw new ArgumentNullException(nameof(budgetPermissionRequestRepository));

    private readonly IUserContextAccessor _userContextAccessor =
        userContextAccessor ?? throw new ArgumentNullException(nameof(userContextAccessor));

    public async Task<IReadOnlyCollection<GetBudgetPermissionRequestsResponse>?> HandleAsync(
        GetBudgetPermissionRequestsQuery query, CancellationToken cancellationToken = default)
    {
        (Guid? budgetId, Guid? participantId, bool? forCurrentUser, GetBudgetPermissionRequestsRequestStatus? status,
            GetBudgetPermissionRequestsRequestPermissionType? permissionType) = query;

        if (forCurrentUser is true)
        {
            participantId = Guid.TryParse(_userContextAccessor.Get()?.UserId, out Guid userId) ? userId : null;
        }

        BudgetPermissionRequestFilter filter = new()
        {
            BudgetId = budgetId,
            ParticipantId = participantId,
            Status = status.HasValue ? GetBudgetPermissionRequestsRequestMap.MapTo(status.Value) : null,
            PermissionType = permissionType.HasValue
                ? GetBudgetPermissionRequestsRequestMap.MapTo(permissionType.Value)
                : null
        };

        IReadOnlyCollection<BudgetPermissionRequest> budgetPermissionRequests =
            await _budgetPermissionRequestRepository.Browse(filter, cancellationToken);

        IReadOnlyCollection<GetBudgetPermissionRequestsResponse> budgetPermissionRequestsResponse =
            GetBudgetPermissionRequestsResponseMap.MapTo(budgetPermissionRequests);

        return budgetPermissionRequestsResponse;
    }
}
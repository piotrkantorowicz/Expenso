using Expenso.BudgetSharing.Application.Read.GetBudgetPermissionRequests.DTO.Requests;
using Expenso.BudgetSharing.Application.Read.GetBudgetPermissionRequests.DTO.Requests.Maps;
using Expenso.BudgetSharing.Application.Read.GetBudgetPermissionRequests.DTO.Responses;
using Expenso.BudgetSharing.Application.Read.GetBudgetPermissionRequests.DTO.Responses.Maps;
using Expenso.BudgetSharing.Application.Read.Shared.QueryStore;
using Expenso.BudgetSharing.Application.Read.Shared.QueryStore.Filters;
using Expenso.BudgetSharing.Domain.BudgetPermissionRequests;
using Expenso.Shared.Queries;
using Expenso.Shared.UserContext;

namespace Expenso.BudgetSharing.Application.Read.GetBudgetPermissionRequests;

internal sealed class GetBudgetPermissionRequestsQueryHandler(
    IBudgetPermissionRequestQueryStore budgetPermissionRequestStore,
    IUserContextAccessor userContextAccessor)
    : IQueryHandler<GetBudgetPermissionRequestsQuery, IReadOnlyCollection<GetBudgetPermissionRequestsResponse>>
{
    private readonly IBudgetPermissionRequestQueryStore _budgetPermissionRequestStore = budgetPermissionRequestStore ??
        throw new ArgumentNullException(nameof(budgetPermissionRequestStore));

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
            await _budgetPermissionRequestStore.Browse(filter, cancellationToken);

        IReadOnlyCollection<GetBudgetPermissionRequestsResponse> budgetPermissionRequestsResponse =
            GetBudgetPermissionRequestsResponseMap.MapTo(budgetPermissionRequests);

        return budgetPermissionRequestsResponse;
    }
}
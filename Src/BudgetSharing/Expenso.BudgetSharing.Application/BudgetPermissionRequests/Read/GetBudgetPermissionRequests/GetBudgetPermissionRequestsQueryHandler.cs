using Expenso.BudgetSharing.Application.BudgetPermissionRequests.Read.GetBudgetPermissionRequests.DTO.Request;
using Expenso.BudgetSharing.Application.BudgetPermissionRequests.Read.GetBudgetPermissionRequests.DTO.Request.Maps;
using Expenso.BudgetSharing.Application.BudgetPermissionRequests.Read.GetBudgetPermissionRequests.DTO.Response;
using Expenso.BudgetSharing.Application.BudgetPermissionRequests.Read.GetBudgetPermissionRequests.DTO.Response.Maps;
using Expenso.BudgetSharing.Application.Shared.QueryStore;
using Expenso.BudgetSharing.Application.Shared.QueryStore.Filters;
using Expenso.BudgetSharing.Domain.BudgetPermissionRequests;
using Expenso.BudgetSharing.Domain.Shared.Model.ValueObjects;
using Expenso.Shared.Queries;
using Expenso.Shared.System.Types.ExecutionContext;

namespace Expenso.BudgetSharing.Application.BudgetPermissionRequests.Read.GetBudgetPermissionRequests;

internal sealed class GetBudgetPermissionRequestsQueryHandler(
    IBudgetPermissionRequestQueryStore budgetPermissionRequestStore,
    IExecutionContextAccessor executionContextAccessor)
    : IQueryHandler<GetBudgetPermissionRequestsQuery, IReadOnlyCollection<GetBudgetPermissionRequestsResponse>>
{
    private readonly IBudgetPermissionRequestQueryStore _budgetPermissionRequestStore = budgetPermissionRequestStore ??
        throw new ArgumentNullException(nameof(budgetPermissionRequestStore));

    private readonly IExecutionContextAccessor _executionContextAccessor =
        executionContextAccessor ?? throw new ArgumentNullException(nameof(executionContextAccessor));

    public async Task<IReadOnlyCollection<GetBudgetPermissionRequestsResponse>?> HandleAsync(
        GetBudgetPermissionRequestsQuery query, CancellationToken cancellationToken)
    {
        (_, Guid? budgetId, Guid? participantId, bool? forCurrentUser,
            GetBudgetPermissionRequestsRequest_Status? status,
            GetBudgetPermissionRequestsRequest_PermissionType? permissionType) = query;

        if (forCurrentUser is true)
        {
            participantId = Guid.TryParse(_executionContextAccessor.Get()?.UserContext?.UserId, out Guid userId)
                ? userId
                : null;
        }

        BudgetPermissionRequestFilter filter = new()
        {
            BudgetId = BudgetId.Nullable(budgetId),
            ParticipantId = PersonId.Nullable(participantId),
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
using Expenso.BudgetSharing.Application.BudgetPermissionRequests.Read.GetBudgetPermissionRequests.DTO.Request;
using Expenso.BudgetSharing.Application.BudgetPermissionRequests.Read.GetBudgetPermissionRequests.DTO.Request.Maps;
using Expenso.BudgetSharing.Application.BudgetPermissionRequests.Read.GetBudgetPermissionRequests.DTO.Response;
using Expenso.BudgetSharing.Application.BudgetPermissionRequests.Read.GetBudgetPermissionRequests.DTO.Response.Maps;
using Expenso.BudgetSharing.Application.Shared.QueryStore;
using Expenso.BudgetSharing.Application.Shared.QueryStore.Filters;
using Expenso.BudgetSharing.Domain.BudgetPermissionRequests;
using Expenso.BudgetSharing.Domain.Shared.ValueObjects;
using Expenso.Shared.Queries;
using Expenso.Shared.System.Types.ExecutionContext;

namespace Expenso.BudgetSharing.Application.BudgetPermissionRequests.Read.GetBudgetPermissionRequests;

internal sealed class GetBudgetPermissionRequestsQueryHandler : IQueryHandler<GetBudgetPermissionRequestsQuery,
    IReadOnlyCollection<GetBudgetPermissionRequestsResponse>>
{
    private readonly IBudgetPermissionRequestQueryStore _budgetPermissionRequestStore;
    private readonly IExecutionContextAccessor _executionContextAccessor;

    public GetBudgetPermissionRequestsQueryHandler(IBudgetPermissionRequestQueryStore budgetPermissionRequestStore,
        IExecutionContextAccessor executionContextAccessor)
    {
        _budgetPermissionRequestStore = budgetPermissionRequestStore ??
                                        throw new ArgumentNullException(
                                            paramName: nameof(budgetPermissionRequestStore));

        _executionContextAccessor = executionContextAccessor ??
                                    throw new ArgumentNullException(paramName: nameof(executionContextAccessor));
    }

    public async Task<IReadOnlyCollection<GetBudgetPermissionRequestsResponse>?> HandleAsync(
        GetBudgetPermissionRequestsQuery query, CancellationToken cancellationToken)
    {
        (_, Guid? budgetId, Guid? participantId, Guid? ownerId, bool? forCurrentUser,
            GetBudgetPermissionRequestsRequest_Status? status,
            GetBudgetPermissionRequestsRequest_PermissionType? permissionType) = query;

        if (forCurrentUser is true)
        {
            participantId =
                Guid.TryParse(input: _executionContextAccessor.Get()?.UserContext?.UserId, result: out Guid userId)
                    ? userId
                    : null;
        }

        BudgetPermissionRequestFilter filter = new()
        {
            BudgetId = BudgetId.Nullable(value: budgetId),
            ParticipantId = PersonId.Nullable(value: participantId),
            OwnerId = PersonId.Nullable(value: ownerId),
            Status = status.HasValue ? GetBudgetPermissionRequestsRequestMap.MapTo(status: status.Value) : null,
            PermissionType = permissionType.HasValue
                ? GetBudgetPermissionRequestsRequestMap.MapTo(permissionType: permissionType.Value)
                : null
        };

        IReadOnlyCollection<BudgetPermissionRequest> budgetPermissionRequests =
            await _budgetPermissionRequestStore.Browse(filter: filter, cancellationToken: cancellationToken);

        IReadOnlyCollection<GetBudgetPermissionRequestsResponse> budgetPermissionRequestsResponse =
            GetBudgetPermissionRequestsResponseMap.MapTo(budgetPermissionRequests: budgetPermissionRequests);

        return budgetPermissionRequestsResponse;
    }
}
using Expenso.BudgetSharing.Application.BudgetPermissionRequests.Read.GetBudgetPermissionRequests.DTO.Maps;
using Expenso.BudgetSharing.Application.BudgetPermissionRequests.Read.GetBudgetPermissionRequests.DTO.Response;
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
        Guid? participantId = query.Payload?.ParticipantId;

        if (query.Payload?.ForCurrentUser is true)
        {
            participantId =
                Guid.TryParse(input: _executionContextAccessor.Get()?.UserContext?.UserId, result: out Guid userId)
                    ? userId
                    : null;
        }

        BudgetPermissionRequestFilter filter = new()
        {
            BudgetId = BudgetId.Nullable(value: query.Payload?.BudgetId),
            ParticipantId = PersonId.Nullable(value: participantId),
            OwnerId = PersonId.Nullable(value: query.Payload?.OwnerId),
            Status = GetBudgetPermissionRequestsRequestMap.MapTo(status: query.Payload?.Status),
            PermissionType = GetBudgetPermissionRequestsRequestMap.MapTo(permissionType: query.Payload?.PermissionType)
        };

        IReadOnlyCollection<BudgetPermissionRequest> budgetPermissionRequests =
            await _budgetPermissionRequestStore.Browse(filter: filter, cancellationToken: cancellationToken);

        IReadOnlyCollection<GetBudgetPermissionRequestsResponse> budgetPermissionRequestsResponse =
            GetBudgetPermissionRequestsResponseMap.MapTo(budgetPermissionRequests: budgetPermissionRequests);

        return budgetPermissionRequestsResponse;
    }
}
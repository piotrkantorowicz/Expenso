using Expenso.BudgetSharing.Application.BudgetPermissions.Read.GetBudgetPermissions.DTO.Maps;
using Expenso.BudgetSharing.Application.Shared.QueryStore;
using Expenso.BudgetSharing.Application.Shared.QueryStore.Filters;
using Expenso.BudgetSharing.Domain.BudgetPermissions;
using Expenso.BudgetSharing.Domain.Shared.ValueObjects;
using Expenso.BudgetSharing.Shared.DTO.API.GetBudgetPermissions.Response;
using Expenso.Shared.Queries;
using Expenso.Shared.System.Types.ExecutionContext;

namespace Expenso.BudgetSharing.Application.BudgetPermissions.Read.GetBudgetPermissions;

internal sealed class
    GetBudgetPermissionsQueryHandler : IQueryHandler<GetBudgetPermissionsQuery,
    IReadOnlyCollection<GetBudgetPermissionsResponse>>
{
    private readonly IBudgetPermissionQueryStore _budgetPermissionStore;
    private readonly IExecutionContextAccessor _executionContextAccessor;

    public GetBudgetPermissionsQueryHandler(IBudgetPermissionQueryStore budgetPermissionStore,
        IExecutionContextAccessor executionContextAccessor)
    {
        _budgetPermissionStore = budgetPermissionStore ??
                                 throw new ArgumentNullException(paramName: nameof(budgetPermissionStore));

        _executionContextAccessor = executionContextAccessor ??
                                    throw new ArgumentNullException(paramName: nameof(executionContextAccessor));
    }

    public async Task<IReadOnlyCollection<GetBudgetPermissionsResponse>?> HandleAsync(GetBudgetPermissionsQuery query,
        CancellationToken cancellationToken)
    {
        Guid? participantId = query.Payload?.ParticipantId;

        if (query.Payload?.ForCurrentUser is true)
        {
            participantId =
                Guid.TryParse(input: _executionContextAccessor.Get()?.UserContext?.UserId, result: out Guid userId)
                    ? userId
                    : null;
        }

        BudgetPermissionFilter filter = new()
        {
            BudgetId = BudgetId.Nullable(value: query.Payload?.BudgetId),
            OwnerId = PersonId.Nullable(value: query.Payload?.OwnerId),
            ParticipantId = PersonId.Nullable(value: participantId),
            PermissionType = GetBudgetPermissionsRequestMap.MapTo(permissionType: query.Payload?.PermissionType)
        };

        IReadOnlyList<BudgetPermission> budgetPermissions =
            await _budgetPermissionStore.BrowseAsync(filter: filter, cancellationToken: cancellationToken);

        IReadOnlyCollection<GetBudgetPermissionsResponse> budgetPermissionsResponse =
            GetBudgetPermissionsResponseMap.MapTo(budgetPermissions: budgetPermissions);

        return budgetPermissionsResponse;
    }
}
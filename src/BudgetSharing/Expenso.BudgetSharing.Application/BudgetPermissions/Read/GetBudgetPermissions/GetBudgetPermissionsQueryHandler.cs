using Expenso.BudgetSharing.Application.BudgetPermissions.Read.GetBudgetPermissions.DTO.Maps;
using Expenso.BudgetSharing.Application.Shared.QueryStore;
using Expenso.BudgetSharing.Application.Shared.QueryStore.Filters;
using Expenso.BudgetSharing.Domain.BudgetPermissions;
using Expenso.BudgetSharing.Domain.Shared.ValueObjects;
using Expenso.BudgetSharing.Shared.DTO.API.BudgetPermissions.GetBudgetPermissions.Response;
using Expenso.Shared.Database.Paging;
using Expenso.Shared.Queries;
using Expenso.Shared.System.Types.ExecutionContext;
using Expenso.Shared.System.Types.Paging;

namespace Expenso.BudgetSharing.Application.BudgetPermissions.Read.GetBudgetPermissions;

internal sealed class
    GetBudgetPermissionsQueryHandler : IQueryHandler<GetBudgetPermissionsQuery,
    IPagedList<GetBudgetPermissionsResponse>>
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

    public async Task<IPagedList<GetBudgetPermissionsResponse>?> HandleAsync(GetBudgetPermissionsQuery query,
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

        BudgetPermissionQuerySpecification querySpecification = new()
        {
            BudgetId = BudgetId.Nullable(value: query.Payload?.BudgetId),
            OwnerId = PersonId.Nullable(value: query.Payload?.OwnerId),
            BudgetCode = BudgetCode.Nullable(value: query.Payload?.BudgetCode),
            ParticipantId = PersonId.Nullable(value: participantId),
            PermissionTypes = GetBudgetPermissionsRequestMap.MapTo(permissionType: query.Payload?.PermissionType)
        };

        IPagedList<BudgetPermission> budgetPermissions = await _budgetPermissionStore.BrowseAsync(
            querySpecification: querySpecification, pagination: DatabasePagination.New(pagination: query.Pagination),
            sorters: query.Sorters, cancellationToken: cancellationToken);

        IPagedList<GetBudgetPermissionsResponse> budgetPermissionsResponse =
            GetBudgetPermissionsResponseMap.MapTo(budgetPermissions: budgetPermissions);

        return budgetPermissionsResponse;
    }
}
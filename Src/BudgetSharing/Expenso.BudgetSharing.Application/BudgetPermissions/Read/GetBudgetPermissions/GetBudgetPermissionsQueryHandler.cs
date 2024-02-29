using Expenso.BudgetSharing.Application.BudgetPermissions.Read.GetBudgetPermissions.DTO.Request.Maps;
using Expenso.BudgetSharing.Application.BudgetPermissions.Read.GetBudgetPermissions.DTO.Response.Maps;
using Expenso.BudgetSharing.Application.Shared.QueryStore;
using Expenso.BudgetSharing.Application.Shared.QueryStore.Filters;
using Expenso.BudgetSharing.Domain.BudgetPermissions;
using Expenso.BudgetSharing.Domain.Shared.ValueObjects;
using Expenso.BudgetSharing.Proxy.DTO.API.GetBudgetPermissions.Request;
using Expenso.BudgetSharing.Proxy.DTO.API.GetBudgetPermissions.Response;
using Expenso.Shared.Queries;
using Expenso.Shared.System.Types.ExecutionContext;

namespace Expenso.BudgetSharing.Application.BudgetPermissions.Read.GetBudgetPermissions;

internal sealed class GetBudgetPermissionsQueryHandler(
    IBudgetPermissionQueryStore budgetPermissionStore,
    IExecutionContextAccessor executionContextAccessor)
    : IQueryHandler<GetBudgetPermissionsQuery, IReadOnlyCollection<GetBudgetPermissionsResponse>>
{
    private readonly IBudgetPermissionQueryStore _budgetPermissionStore = budgetPermissionStore ??
                                                                          throw new ArgumentNullException(
                                                                              nameof(budgetPermissionStore));

    private readonly IExecutionContextAccessor _executionContextAccessor =
        executionContextAccessor ?? throw new ArgumentNullException(nameof(executionContextAccessor));

    public async Task<IReadOnlyCollection<GetBudgetPermissionsResponse>?> HandleAsync(GetBudgetPermissionsQuery query,
        CancellationToken cancellationToken)
    {
        (_, Guid? budgetId, Guid? ownerId, Guid? participantId, bool? forCurrentUser,
            GetBudgetPermissionsRequestPermissionType? permissionType) = query;

        if (forCurrentUser is true)
        {
            participantId = Guid.TryParse(_executionContextAccessor.Get()?.UserContext?.UserId, out Guid userId)
                ? userId
                : null;
        }

        BudgetPermissionFilter filter = new()
        {
            BudgetId = BudgetId.Nullable(budgetId),
            OwnerId = PersonId.Nullable(ownerId),
            ParticipantId = PersonId.Nullable(participantId),
            PermissionType = permissionType.HasValue ? GetBudgetPermissionsRequestMap.MapTo(permissionType.Value) : null
        };

        IReadOnlyList<BudgetPermission> budgetPermissions =
            await _budgetPermissionStore.BrowseAsync(filter, cancellationToken);

        IReadOnlyCollection<GetBudgetPermissionsResponse> budgetPermissionsResponse =
            GetBudgetPermissionsResponseMap.MapTo(budgetPermissions);

        return budgetPermissionsResponse;
    }
}
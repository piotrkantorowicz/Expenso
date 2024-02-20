using Expenso.BudgetSharing.Application.Read.GetBudgetPermissions.DTO.Requests;
using Expenso.BudgetSharing.Application.Read.GetBudgetPermissions.DTO.Requests.Maps;
using Expenso.BudgetSharing.Application.Read.GetBudgetPermissions.DTO.Responses;
using Expenso.BudgetSharing.Application.Read.GetBudgetPermissions.DTO.Responses.Maps;
using Expenso.BudgetSharing.Application.Read.Shared.QueryStore;
using Expenso.BudgetSharing.Application.Read.Shared.QueryStore.Filters;
using Expenso.BudgetSharing.Domain.BudgetPermissions;
using Expenso.BudgetSharing.Domain.Shared.Model.ValueObjects;
using Expenso.Shared.Queries;
using Expenso.Shared.System.Types.ExecutionContext;

namespace Expenso.BudgetSharing.Application.Read.GetBudgetPermissions;

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
        CancellationToken cancellationToken = default)
    {
        (_, Guid? budgetId, Guid? ownerId, Guid? participantId, bool? forCurrentUser, bool? includePermissions,
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
            PermissionType =
                permissionType.HasValue ? GetBudgetPermissionsRequestMap.MapTo(permissionType.Value) : null,
            IncludePermissions = includePermissions
        };

        IReadOnlyList<BudgetPermission> budgetPermissions =
            await _budgetPermissionStore.BrowseAsync(filter, cancellationToken);

        IReadOnlyCollection<GetBudgetPermissionsResponse> budgetPermissionsResponse =
            GetBudgetPermissionsResponseMap.MapTo(budgetPermissions);

        return budgetPermissionsResponse;
    }
}
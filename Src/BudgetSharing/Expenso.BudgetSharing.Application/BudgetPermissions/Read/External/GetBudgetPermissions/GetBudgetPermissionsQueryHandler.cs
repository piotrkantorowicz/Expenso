using Expenso.BudgetSharing.Application.BudgetPermissions.Read.External.GetBudgetPermissions.DTO;
using Expenso.BudgetSharing.Application.Shared.QueryStore;
using Expenso.BudgetSharing.Application.Shared.QueryStore.Filters;
using Expenso.BudgetSharing.Domain.BudgetPermissions;
using Expenso.BudgetSharing.Domain.BudgetPermissions.ValueObjects;
using Expenso.BudgetSharing.Domain.Shared.Model.ValueObjects;
using Expenso.BudgetSharing.Proxy.DTO.API.GetBudgetPermissions.Responses;
using Expenso.Shared.Queries;

namespace Expenso.BudgetSharing.Application.BudgetPermissions.Read.External.GetBudgetPermissions;

internal sealed class GetBudgetPermissionsQueryHandler(IBudgetPermissionQueryStore budgetPermissionQueryStore)
    : IQueryHandler<GetBudgetPermissionsQuery, IReadOnlyCollection<GetBudgetPermissionsResponse>>
{
    private readonly IBudgetPermissionQueryStore _budgetPermissionQueryStore = budgetPermissionQueryStore ??
                                                                               throw new ArgumentNullException(
                                                                                   nameof(budgetPermissionQueryStore));

    public async Task<IReadOnlyCollection<GetBudgetPermissionsResponse>?> HandleAsync(GetBudgetPermissionsQuery query,
        CancellationToken cancellationToken)
    {
        BudgetPermissionFilter filter = new()
        {
            Id = BudgetPermissionId.Nullable(query.BudgetPermissionId),
            BudgetId = BudgetId.Nullable(query.BudgetId),
            OwnerId = PersonId.Nullable(query.OwnerId),
            ParticipantId = PersonId.Nullable(query.ParticipantId)
        };

        IReadOnlyList<BudgetPermission> budgetPermissions =
            await _budgetPermissionQueryStore.BrowseAsync(filter, cancellationToken);

        return GetBudgetPermissionMap.MapTo(budgetPermissions);
    }
}
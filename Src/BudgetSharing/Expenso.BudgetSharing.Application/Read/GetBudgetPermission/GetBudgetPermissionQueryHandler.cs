using Expenso.BudgetSharing.Application.Read.GetBudgetPermission.DTO.Responses;
using Expenso.BudgetSharing.Application.Read.GetBudgetPermission.DTO.Responses.Maps;
using Expenso.BudgetSharing.Application.Read.Shared.QueryStore;
using Expenso.BudgetSharing.Application.Read.Shared.QueryStore.Filters;
using Expenso.BudgetSharing.Domain.BudgetPermissions;
using Expenso.BudgetSharing.Domain.BudgetPermissions.ValueObjects;
using Expenso.Shared.Queries;
using Expenso.Shared.System.Types.Exceptions;

namespace Expenso.BudgetSharing.Application.Read.GetBudgetPermission;

internal sealed class GetBudgetPermissionQueryHandler(IBudgetPermissionQueryStore budgetPermissionStore)
    : IQueryHandler<GetBudgetPermissionQuery, GetBudgetPermissionResponse>
{
    private readonly IBudgetPermissionQueryStore _budgetPermissionStore = budgetPermissionStore ??
                                                                          throw new ArgumentNullException(
                                                                              nameof(budgetPermissionStore));

    public async Task<GetBudgetPermissionResponse?> HandleAsync(GetBudgetPermissionQuery query,
        CancellationToken cancellationToken = default)
    {
        (_, Guid budgetPermissionId, bool? includePermissions) = query;

        BudgetPermissionFilter filter = new()
        {
            Id = BudgetPermissionId.Nullable(budgetPermissionId),
            IncludePermissions = includePermissions
        };

        BudgetPermission? budgetPermission = await _budgetPermissionStore.SingleAsync(filter, cancellationToken);

        if (budgetPermission is null)
        {
            throw new NotFoundException($" Budget permission with id {query.BudgetPermissionId} hasn't been found");
        }

        GetBudgetPermissionResponse budgetPermissionResponse = GetBudgetPermissionResponseMap.MapTo(budgetPermission);

        return budgetPermissionResponse;
    }
}
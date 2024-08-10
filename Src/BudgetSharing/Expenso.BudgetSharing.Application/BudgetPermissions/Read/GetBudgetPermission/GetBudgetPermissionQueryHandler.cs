using Expenso.BudgetSharing.Application.BudgetPermissions.Read.GetBudgetPermission.DTO.Response;
using Expenso.BudgetSharing.Application.BudgetPermissions.Read.GetBudgetPermission.DTO.Response.Maps;
using Expenso.BudgetSharing.Application.Shared.QueryStore;
using Expenso.BudgetSharing.Application.Shared.QueryStore.Filters;
using Expenso.BudgetSharing.Domain.BudgetPermissions;
using Expenso.BudgetSharing.Domain.BudgetPermissions.ValueObjects;
using Expenso.Shared.Queries;
using Expenso.Shared.System.Types.Exceptions;

namespace Expenso.BudgetSharing.Application.BudgetPermissions.Read.GetBudgetPermission;

internal sealed class GetBudgetPermissionQueryHandler(IBudgetPermissionQueryStore budgetPermissionStore)
    : IQueryHandler<GetBudgetPermissionQuery, GetBudgetPermissionResponse>
{
    private readonly IBudgetPermissionQueryStore _budgetPermissionStore = budgetPermissionStore ??
                                                                          throw new ArgumentNullException(
                                                                              paramName: nameof(budgetPermissionStore));

    public async Task<GetBudgetPermissionResponse?> HandleAsync(GetBudgetPermissionQuery query,
        CancellationToken cancellationToken)
    {
        (_, Guid budgetPermissionId) = query;

        BudgetPermissionFilter filter = new()
        {
            Id = BudgetPermissionId.Nullable(value: budgetPermissionId)
        };

        BudgetPermission? budgetPermission =
            await _budgetPermissionStore.SingleAsync(filter: filter, cancellationToken: cancellationToken);

        if (budgetPermission is null)
        {
            throw new NotFoundException(
                message: $" Budget permission with id {query.BudgetPermissionId} hasn't been found.");
        }

        GetBudgetPermissionResponse budgetPermissionResponse =
            GetBudgetPermissionResponseMap.MapTo(budgetPermission: budgetPermission);

        return budgetPermissionResponse;
    }
}
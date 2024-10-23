using Expenso.BudgetSharing.Application.BudgetPermissions.Read.GetBudgetPermission.DTO.Maps;
using Expenso.BudgetSharing.Application.BudgetPermissions.Read.GetBudgetPermission.DTO.Response;
using Expenso.BudgetSharing.Application.Shared.QueryStore;
using Expenso.BudgetSharing.Application.Shared.QueryStore.Filters;
using Expenso.BudgetSharing.Domain.BudgetPermissions;
using Expenso.BudgetSharing.Domain.BudgetPermissions.ValueObjects;
using Expenso.Shared.Queries;
using Expenso.Shared.System.Types.Exceptions;

namespace Expenso.BudgetSharing.Application.BudgetPermissions.Read.GetBudgetPermission;

internal sealed class
    GetBudgetPermissionQueryHandler : IQueryHandler<GetBudgetPermissionQuery, GetBudgetPermissionResponse>
{
    private readonly IBudgetPermissionQueryStore _budgetPermissionStore;

    public GetBudgetPermissionQueryHandler(IBudgetPermissionQueryStore budgetPermissionStore)
    {
        _budgetPermissionStore = budgetPermissionStore ??
                                 throw new ArgumentNullException(paramName: nameof(budgetPermissionStore));
    }

    public async Task<GetBudgetPermissionResponse?> HandleAsync(GetBudgetPermissionQuery query,
        CancellationToken cancellationToken)
    {
        BudgetPermissionFilter filter = new()
        {
            Id = BudgetPermissionId.Nullable(value: query.Payload?.BudgetPermissionId)
        };

        BudgetPermission? budgetPermission =
            await _budgetPermissionStore.SingleAsync(filter: filter, cancellationToken: cancellationToken);

        if (budgetPermission is null)
        {
            throw new NotFoundException(message: $" Budget permission with ID {query.Payload} hasn't been found");
        }

        GetBudgetPermissionResponse budgetPermissionResponse =
            GetBudgetPermissionResponseMap.MapTo(budgetPermission: budgetPermission);

        return budgetPermissionResponse;
    }
}
using Expenso.BudgetSharing.Application.DTO.GetBudgetPermission.Responses;
using Expenso.BudgetSharing.Application.DTO.GetBudgetPermission.Responses.Maps;
using Expenso.BudgetSharing.Application.QueryStore.Filters;
using Expenso.BudgetSharing.Application.QueryStore.Repositories;
using Expenso.BudgetSharing.Domain.BudgetPermissions.Model;
using Expenso.Shared.Queries;
using Expenso.Shared.Types.Exceptions;

namespace Expenso.BudgetSharing.Application.Queries.GetBudgetPermission;

internal sealed class GetBudgetPermissionQueryHandler(IBudgetPermissionReadRepository budgetPermissionRepository)
    : IQueryHandler<GetBudgetPermissionQuery, GetBudgetPermissionResponse>
{
    private readonly IBudgetPermissionReadRepository _budgetPermissionRepository = budgetPermissionRepository ??
        throw new ArgumentNullException(nameof(budgetPermissionRepository));

    public async Task<GetBudgetPermissionResponse?> HandleAsync(GetBudgetPermissionQuery query,
        CancellationToken cancellationToken = default)
    {
        (Guid id, bool? includePermissions) = query;

        BudgetPermissionFilter filter = new()
        {
            Id = id,
            IncludePermissions = includePermissions
        };

        BudgetPermission? budgetPermission = await _budgetPermissionRepository.SingleAsync(filter, cancellationToken);

        if (budgetPermission is null)
        {
            throw new NotFoundException($" Budget permission with id {query.Id} hasn't been found");
        }

        GetBudgetPermissionResponse budgetPermissionResponse = GetBudgetPermissionResponseMap.MapTo(budgetPermission);

        return budgetPermissionResponse;
    }
}
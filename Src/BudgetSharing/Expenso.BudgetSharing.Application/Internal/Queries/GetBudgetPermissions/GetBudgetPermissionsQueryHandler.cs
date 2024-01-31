using Expenso.BudgetSharing.Proxy.DTO.API.GetBudgetPermissions.Responses;
using Expenso.Shared.Queries;

namespace Expenso.BudgetSharing.Application.Internal.Queries.GetBudgetPermissions;

internal sealed class
    GetBudgetPermissionsQueryHandler : IQueryHandler<GetBudgetPermissionsQuery,
    IReadOnlyCollection<GetBudgetPermissionsResponse>>
{
    public Task<IReadOnlyCollection<GetBudgetPermissionsResponse>?> HandleAsync(GetBudgetPermissionsQuery query,
        CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
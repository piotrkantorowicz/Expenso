using Expenso.BudgetSharing.Application.BudgetPermissionRequests.Read.GetBudgetPermissionRequest.DTO.Response;
using Expenso.BudgetSharing.Application.BudgetPermissionRequests.Read.GetBudgetPermissionRequest.DTO.Response.Maps;
using Expenso.BudgetSharing.Application.Shared.QueryStore;
using Expenso.BudgetSharing.Application.Shared.QueryStore.Filters;
using Expenso.BudgetSharing.Domain.BudgetPermissionRequests;
using Expenso.BudgetSharing.Domain.BudgetPermissionRequests.ValueObjects;
using Expenso.Shared.Queries;
using Expenso.Shared.System.Types.Exceptions;

namespace Expenso.BudgetSharing.Application.BudgetPermissionRequests.Read.GetBudgetPermissionRequest;

internal sealed class GetBudgetPermissionRequestQueryHandler(
    IBudgetPermissionRequestQueryStore budgetPermissionRequestStore)
    : IQueryHandler<GetBudgetPermissionRequestQuery, GetBudgetPermissionRequestResponse>
{
    private readonly IBudgetPermissionRequestQueryStore _budgetPermissionRequestStore = budgetPermissionRequestStore ??
        throw new ArgumentNullException(nameof(budgetPermissionRequestStore));

    public async Task<GetBudgetPermissionRequestResponse?> HandleAsync(GetBudgetPermissionRequestQuery query,
        CancellationToken cancellationToken)
    {
        BudgetPermissionRequestFilter filter = new()
        {
            Id = BudgetPermissionRequestId.Nullable(query.BudgetPermissionRequestId)
        };

        BudgetPermissionRequest? budgetPermissionRequest =
            await _budgetPermissionRequestStore.SingleAsync(filter, cancellationToken);

        if (budgetPermissionRequest is null)
        {
            throw new NotFoundException(
                $"Budget permission request with id {query.BudgetPermissionRequestId} hasn't been found.");
        }

        GetBudgetPermissionRequestResponse budgetPermissionRequestResponse =
            GetBudgetPermissionRequestResponseMap.MapTo(budgetPermissionRequest);

        return budgetPermissionRequestResponse;
    }
}
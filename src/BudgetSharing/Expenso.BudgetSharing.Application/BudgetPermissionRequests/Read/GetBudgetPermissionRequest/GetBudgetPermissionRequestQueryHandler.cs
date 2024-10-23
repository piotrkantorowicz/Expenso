using Expenso.BudgetSharing.Application.BudgetPermissionRequests.Read.GetBudgetPermissionRequest.DTO.Maps;
using Expenso.BudgetSharing.Application.BudgetPermissionRequests.Read.GetBudgetPermissionRequest.DTO.Response;
using Expenso.BudgetSharing.Application.Shared.QueryStore;
using Expenso.BudgetSharing.Application.Shared.QueryStore.Filters;
using Expenso.BudgetSharing.Domain.BudgetPermissionRequests;
using Expenso.BudgetSharing.Domain.BudgetPermissionRequests.ValueObjects;
using Expenso.Shared.Queries;
using Expenso.Shared.System.Types.Exceptions;

namespace Expenso.BudgetSharing.Application.BudgetPermissionRequests.Read.GetBudgetPermissionRequest;

internal sealed class
    GetBudgetPermissionRequestQueryHandler : IQueryHandler<GetBudgetPermissionRequestQuery,
    GetBudgetPermissionRequestResponse>
{
    private readonly IBudgetPermissionRequestQueryStore _budgetPermissionRequestStore;

    public GetBudgetPermissionRequestQueryHandler(IBudgetPermissionRequestQueryStore budgetPermissionRequestStore)
    {
        _budgetPermissionRequestStore = budgetPermissionRequestStore ??
                                        throw new ArgumentNullException(
                                            paramName: nameof(budgetPermissionRequestStore));
    }

    public async Task<GetBudgetPermissionRequestResponse?> HandleAsync(GetBudgetPermissionRequestQuery query,
        CancellationToken cancellationToken)
    {
        BudgetPermissionRequestFilter filter = new()
        {
            Id = BudgetPermissionRequestId.Nullable(value: query.Payload?.BudgetPermissionRequestId)
        };

        BudgetPermissionRequest? budgetPermissionRequest =
            await _budgetPermissionRequestStore.SingleAsync(filter: filter, cancellationToken: cancellationToken);

        if (budgetPermissionRequest is null)
        {
            throw new NotFoundException(
                message:
                $"Budget permission request with ID {query.Payload?.BudgetPermissionRequestId} hasn't been found");
        }

        GetBudgetPermissionRequestResponse budgetPermissionRequestResponse =
            GetBudgetPermissionRequestResponseMap.MapTo(budgetPermissionRequest: budgetPermissionRequest);

        return budgetPermissionRequestResponse;
    }
}
using Expenso.BudgetSharing.Application.DTO.GetBudgetPermissionRequest.Responses;
using Expenso.BudgetSharing.Application.DTO.GetBudgetPermissionRequest.Responses.Maps;
using Expenso.BudgetSharing.Application.QueryStore.Filters;
using Expenso.BudgetSharing.Application.QueryStore.Repositories;
using Expenso.BudgetSharing.Domain.BudgetPermissionRequests.Model;
using Expenso.Shared.Queries;
using Expenso.Shared.Types.Exceptions;

namespace Expenso.BudgetSharing.Application.Queries.GetBudgetPermissionRequest;

internal sealed class GetBudgetPermissionRequestQueryHandler(
    IBudgetPermissionRequestReadRepository budgetPermissionRequestRepository)
    : IQueryHandler<GetBudgetPermissionRequestQuery, GetBudgetPermissionRequestResponse>
{
    private readonly IBudgetPermissionRequestReadRepository _budgetPermissionRequestRepository =
        budgetPermissionRequestRepository ?? throw new ArgumentNullException(nameof(budgetPermissionRequestRepository));

    public async Task<GetBudgetPermissionRequestResponse?> HandleAsync(GetBudgetPermissionRequestQuery query,
        CancellationToken cancellationToken = default)
    {
        BudgetPermissionRequestFilter filter = new()
        {
            Id = query.Id
        };

        BudgetPermissionRequest? budgetPermissionRequest =
            await _budgetPermissionRequestRepository.SingleAsync(filter, cancellationToken);

        if (budgetPermissionRequest is null)
        {
            throw new NotFoundException($"Budget permission request with id {query.Id} hasn't been found");
        }

        GetBudgetPermissionRequestResponse budgetPermissionRequestResponse =
            GetBudgetPermissionRequestResponseMap.MapTo(budgetPermissionRequest);

        return budgetPermissionRequestResponse;
    }
}
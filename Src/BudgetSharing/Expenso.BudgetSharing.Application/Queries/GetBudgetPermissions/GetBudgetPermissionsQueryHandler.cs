using Expenso.BudgetSharing.Application.DTO.GetBudgetPermissions.Requests;
using Expenso.BudgetSharing.Application.DTO.GetBudgetPermissions.Requests.Maps;
using Expenso.BudgetSharing.Application.DTO.GetBudgetPermissions.Responses;
using Expenso.BudgetSharing.Application.DTO.GetBudgetPermissions.Responses.Maps;
using Expenso.BudgetSharing.Application.QueryStore.Filters;
using Expenso.BudgetSharing.Application.QueryStore.Repositories;
using Expenso.BudgetSharing.Domain.BudgetPermissions.Model;
using Expenso.Shared.Queries;
using Expenso.Shared.UserContext;

namespace Expenso.BudgetSharing.Application.Queries.GetBudgetPermissions;

internal sealed class GetBudgetPermissionsQueryHandler(
    IBudgetPermissionReadRepository budgetPermissionRepository,
    IUserContextAccessor userContextAccessor)
    : IQueryHandler<GetBudgetPermissionsQuery, IReadOnlyCollection<GetBudgetPermissionsResponse>>
{
    private readonly IBudgetPermissionReadRepository _budgetPermissionRepository = budgetPermissionRepository ??
        throw new ArgumentNullException(nameof(budgetPermissionRepository));

    private readonly IUserContextAccessor _userContextAccessor =
        userContextAccessor ?? throw new ArgumentNullException(nameof(userContextAccessor));

    public async Task<IReadOnlyCollection<GetBudgetPermissionsResponse>?> HandleAsync(GetBudgetPermissionsQuery query,
        CancellationToken cancellationToken = default)
    {
        (Guid? id, Guid? budgetId, Guid? participantId, bool? forCurrentUser, bool? includePermissions,
            GetBudgetPermissionsRequestPermissionType? permissionType) = query;

        if (forCurrentUser is true)
        {
            participantId = Guid.TryParse(_userContextAccessor.Get()?.UserId, out Guid userId) ? userId : null;
        }

        BudgetPermissionFilter filter = new()
        {
            Id = id,
            BudgetId = budgetId,
            ParticipantId = participantId,
            PermissionType =
                permissionType.HasValue ? GetBudgetPermissionsRequestMap.MapTo(permissionType.Value) : null,
            IncludePermissions = includePermissions
        };

        IReadOnlyList<BudgetPermission> budgetPermissions =
            await _budgetPermissionRepository.BrowseAsync(filter, cancellationToken);

        IReadOnlyCollection<GetBudgetPermissionsResponse> budgetPermissionsResponse =
            GetBudgetPermissionsResponseMap.MapTo(budgetPermissions);

        return budgetPermissionsResponse;
    }
}
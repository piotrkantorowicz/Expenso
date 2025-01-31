using Expenso.BudgetSharing.Shared.DTO.API.BudgetPermissionRequests.AssignParticipant.Request;
using Expenso.BudgetSharing.Shared.DTO.API.BudgetPermissionRequests.AssignParticipant.Response;
using Expenso.BudgetSharing.Shared.DTO.API.BudgetPermissions.AddPermission.Request;
using Expenso.BudgetSharing.Shared.DTO.API.BudgetPermissions.CreateBudgetPermission.Request;
using Expenso.BudgetSharing.Shared.DTO.API.BudgetPermissions.CreateBudgetPermission.Response;
using Expenso.BudgetSharing.Shared.DTO.API.BudgetPermissions.DeleteBudgetPermission.Request;
using Expenso.BudgetSharing.Shared.DTO.API.BudgetPermissions.GetBudgetPermissions.Request;
using Expenso.BudgetSharing.Shared.DTO.API.BudgetPermissions.GetBudgetPermissions.Response;
using Expenso.Shared.System.Types.Messages.Interfaces;
using Expenso.Shared.System.Types.Ordering;
using Expenso.Shared.System.Types.Paging;

namespace Expenso.BudgetSharing.Shared;

public interface IBudgetSharingProxy
{
    Task<IPagedList<GetBudgetPermissionsResponse>?> GetBudgetPermissionsAsync(GetBudgetPermissionsRequest request,
        Pagination? pagination = null, Sorting? sorting = null, IMessageContext? messageContext = null,
        CancellationToken cancellationToken = default);

    Task<AssignParticipantResponse?> AssignParticipantAsync(AssignParticipantRequest request,
        IMessageContext? messageContext = null, CancellationToken cancellationToken = default);

    Task<CreateBudgetPermissionResponse?> CreateBudgetPermissionAsync(CreateBudgetPermissionRequest request,
        IMessageContext? messageContext = null, CancellationToken cancellationToken = default);

    Task DeleteBudgetPermissionAsync(DeleteBudgetPermissionRequest request, IMessageContext? messageContext = null,
        CancellationToken cancellationToken = default);

    Task AddPermissionAsync(AddPermissionRequest request, IMessageContext? messageContext = null,
        CancellationToken cancellationToken = default);
}
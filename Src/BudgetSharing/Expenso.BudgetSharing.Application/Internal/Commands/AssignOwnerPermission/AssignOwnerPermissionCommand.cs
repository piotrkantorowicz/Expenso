using Expenso.Shared.Commands;

namespace Expenso.BudgetSharing.Application.Internal.Commands.AssignOwnerPermission;

public sealed record AssignOwnerPermissionCommand(
    Guid BudgetPermissionId,
    BudgetSharing.Proxy.DTO.API.AssignOwnerPermission.Requests.AssignOwnerPermission AssignOwnerPermission) : ICommand;
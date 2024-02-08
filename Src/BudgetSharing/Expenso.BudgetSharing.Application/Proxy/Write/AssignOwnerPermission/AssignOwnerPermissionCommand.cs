using Expenso.BudgetSharing.Proxy.DTO.API.AssignOwnerPermission.Requests;
using Expenso.Shared.Commands;

namespace Expenso.BudgetSharing.Application.Proxy.Write.AssignOwnerPermission;

public sealed record AssignOwnerPermissionCommand(
    Guid BudgetPermissionId,
    AssignOwnerPermissionRequest AssignOwnerPermissionRequest) : ICommand;
using Expenso.BudgetSharing.Proxy.DTO.API.AssignOwnerPermission.Requests;
using Expenso.Shared.Commands;
using Expenso.Shared.System.Types.Messages.Interfaces;

namespace Expenso.BudgetSharing.Application.Proxy.Write.AssignOwnerPermission;

public sealed record AssignOwnerPermissionCommand(
    IMessageContext MessageContext,
    Guid BudgetPermissionId,
    AssignOwnerPermissionRequest AssignOwnerPermissionRequest) : ICommand;
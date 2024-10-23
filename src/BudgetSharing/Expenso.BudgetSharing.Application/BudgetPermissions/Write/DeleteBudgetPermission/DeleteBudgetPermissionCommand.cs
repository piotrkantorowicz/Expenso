using Expenso.BudgetSharing.Application.BudgetPermissions.Write.DeleteBudgetPermission.DTO.Request;
using Expenso.Shared.Commands;
using Expenso.Shared.System.Types.Messages.Interfaces;

namespace Expenso.BudgetSharing.Application.BudgetPermissions.Write.DeleteBudgetPermission;

public sealed record DeleteBudgetPermissionCommand(
    IMessageContext MessageContext,
    DeleteBudgetPermissionRequest? Payload) : ICommand;
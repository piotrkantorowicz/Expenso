using Expenso.BudgetSharing.Application.BudgetPermissions.Write.Internal.CreateBudgetPermission.DTO.Request;
using Expenso.Shared.Commands;
using Expenso.Shared.System.Types.Messages.Interfaces;

namespace Expenso.BudgetSharing.Application.BudgetPermissions.Write.Internal.CreateBudgetPermission;

public sealed record CreateBudgetPermissionCommand(
    IMessageContext MessageContext,
    CreateBudgetPermissionRequest AddPermissionRequest) : ICommand;
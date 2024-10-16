using Expenso.BudgetSharing.Shared.DTO.API.CreateBudgetPermission.Request;
using Expenso.Shared.Commands;
using Expenso.Shared.System.Types.Messages.Interfaces;

namespace Expenso.BudgetSharing.Application.BudgetPermissions.Write.CreateBudgetPermission;

public sealed record CreateBudgetPermissionCommand(
    IMessageContext MessageContext,
    CreateBudgetPermissionRequest CreateBudgetPermissionRequest) : ICommand;
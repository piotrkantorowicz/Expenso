using Expenso.BudgetSharing.Proxy.DTO.API.CreateBudgetPermission.Requests;
using Expenso.Shared.Commands;
using Expenso.Shared.System.Types.Messages.Interfaces;

namespace Expenso.BudgetSharing.Application.BudgetPermissions.Write.External.CreateBudgetPermission;

public sealed record CreateBudgetPermissionCommand(
    IMessageContext MessageContext,
    CreateBudgetPermissionRequest CreateBudgetPermissionRequest) : ICommand;
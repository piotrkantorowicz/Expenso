using Expenso.BudgetSharing.Shared.DTO.API.BudgetPermissions.AddPermission.Request;
using Expenso.Shared.Commands;
using Expenso.Shared.System.Types.Messages.Interfaces;

namespace Expenso.BudgetSharing.Application.BudgetPermissions.Write.AddPermission;

public sealed record AddPermissionCommand(IMessageContext MessageContext, AddPermissionRequest? Payload) : ICommand;
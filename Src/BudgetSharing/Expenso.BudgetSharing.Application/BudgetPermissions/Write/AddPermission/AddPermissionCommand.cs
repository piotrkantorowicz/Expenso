using Expenso.BudgetSharing.Application.BudgetPermissions.Write.AddPermission.DTO.Request;
using Expenso.Shared.Commands;
using Expenso.Shared.System.Types.Messages.Interfaces;

namespace Expenso.BudgetSharing.Application.BudgetPermissions.Write.AddPermission;

public sealed record AddPermissionCommand(
    IMessageContext MessageContext,
    Guid BudgetPermissionId,
    Guid ParticipantId,
    AddPermissionRequest AddPermissionRequest) : ICommand;
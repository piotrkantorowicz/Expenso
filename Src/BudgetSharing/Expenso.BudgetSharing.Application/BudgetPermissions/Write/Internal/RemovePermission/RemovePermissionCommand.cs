using Expenso.Shared.Commands;
using Expenso.Shared.System.Types.Messages.Interfaces;

namespace Expenso.BudgetSharing.Application.BudgetPermissions.Write.Internal.RemovePermission;

public sealed record RemovePermissionCommand(
    IMessageContext MessageContext,
    Guid BudgetPermissionId,
    Guid ParticipantId) : ICommand;
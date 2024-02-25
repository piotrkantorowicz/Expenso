using Expenso.Shared.Commands;
using Expenso.Shared.System.Types.Messages.Interfaces;

namespace Expenso.BudgetSharing.Application.BudgetPermissions.Write.External.RestoreBudgetPermission;

public sealed record RestoreBudgetPermissionCommand(IMessageContext MessageContext, Guid BudgetPermissionId) : ICommand;
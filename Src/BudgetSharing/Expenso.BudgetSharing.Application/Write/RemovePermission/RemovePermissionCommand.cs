using Expenso.Shared.Commands;

namespace Expenso.BudgetSharing.Application.Write.RemovePermission;

public sealed record RemovePermissionCommand(Guid BudgetPermissionId, Guid ParticipantId) : ICommand;
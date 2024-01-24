using Expenso.BudgetSharing.Core.Domain.BudgetPermissions.Repositories;
using Expenso.Shared.Commands;

namespace Expenso.BudgetSharing.Core.Application.Commands.AssignParticipant;

internal sealed class AssignParticipantCommandHandler(IBudgetPermissionRepository budgetPermissionRepository)
    : ICommandHandler<AssignParticipantCommand>
{
    private readonly IBudgetPermissionRepository _budgetPermissionRepository = budgetPermissionRepository ??
                                                                               throw new ArgumentNullException(
                                                                                   nameof(budgetPermissionRepository));

    public Task HandleAsync(AssignParticipantCommand command, CancellationToken cancellationToken = default)
    {
        return Task.CompletedTask;
    }
}
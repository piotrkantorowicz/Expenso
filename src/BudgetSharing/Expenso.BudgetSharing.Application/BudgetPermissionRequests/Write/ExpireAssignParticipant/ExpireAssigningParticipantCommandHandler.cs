using Expenso.BudgetSharing.Domain.BudgetPermissionRequests.Services.Interfaces;
using Expenso.Shared.Commands;

namespace Expenso.BudgetSharing.Application.BudgetPermissionRequests.Write.ExpireAssignParticipant;

internal sealed class ExpireAssigningParticipantCommandHandler : ICommandHandler<ExpireAssigningParticipantCommand>
{
    private readonly IBudgetPermissionRequestExpireDomainService _budgetPermissionRequestExpireDomainService;

    public ExpireAssigningParticipantCommandHandler(
        IBudgetPermissionRequestExpireDomainService budgetPermissionRequestExpireDomainService)
    {
        _budgetPermissionRequestExpireDomainService = budgetPermissionRequestExpireDomainService ??
                                                      throw new ArgumentNullException(
                                                          paramName: nameof(
                                                              budgetPermissionRequestExpireDomainService));
    }

    public async Task HandleAsync(ExpireAssigningParticipantCommand command, CancellationToken cancellationToken)
    {
        await _budgetPermissionRequestExpireDomainService.MarkBudgetPermissionRequestAsExpire(
            budgetPermissionRequestId: command.BudgetPermissionRequestId, cancellationToken: cancellationToken);
    }
}
using Expenso.BudgetSharing.Domain.BudgetPermissionRequests.Services.Interfaces;
using Expenso.Shared.Commands;

namespace Expenso.BudgetSharing.Application.BudgetPermissionRequests.Write.ExpireAssigningParticipant;

internal sealed class ExpireAssigningParticipantCommandHandler : ICommandHandler<ExpireAssigningParticipantCommand>
{
    private readonly IBudgetPermissionRequestExpirationDomainService _budgetPermissionRequestExpirationDomainService;

    public ExpireAssigningParticipantCommandHandler(
        IBudgetPermissionRequestExpirationDomainService budgetPermissionRequestExpirationDomainService)
    {
        _budgetPermissionRequestExpirationDomainService = budgetPermissionRequestExpirationDomainService ??
                                                          throw new ArgumentNullException(
                                                              paramName: nameof(
                                                                  budgetPermissionRequestExpirationDomainService));
    }

    public async Task HandleAsync(ExpireAssigningParticipantCommand command, CancellationToken cancellationToken)
    {
        await _budgetPermissionRequestExpirationDomainService.MarkBudgetPermissionRequestAsExpireAsync(
            budgetPermissionRequestId: command.Payload?.BudgetPermissionRequestId,
            cancellationToken: cancellationToken);
    }
}
using Expenso.BudgetSharing.Application.BudgetPermissionRequests.Write.AssignParticipant.DTO.Requests;
using Expenso.BudgetSharing.Application.BudgetPermissionRequests.Write.AssignParticipant.DTO.Requests.Maps;
using Expenso.BudgetSharing.Application.BudgetPermissionRequests.Write.AssignParticipant.DTO.Responses;
using Expenso.BudgetSharing.Domain.BudgetPermissionRequests.Services.Interfaces;
using Expenso.BudgetSharing.Domain.BudgetPermissionRequests.ValueObjects;
using Expenso.Shared.Commands;

namespace Expenso.BudgetSharing.Application.BudgetPermissionRequests.Write.AssignParticipant;

internal sealed class AssignParticipantCommandHandler(IAssignParticipantDomainService assignParticipantDomainService)
    : ICommandHandler<AssignParticipantCommand, AssignParticipantResponse>
{
    private readonly IAssignParticipantDomainService _assignParticipantDomainService = assignParticipantDomainService ??
        throw new ArgumentNullException(nameof(assignParticipantDomainService));

    public async Task<AssignParticipantResponse?> HandleAsync(AssignParticipantCommand command,
        CancellationToken cancellationToken)
    {
        (_,
            (Guid budgetId, Guid participantId, AssignParticipantRequestPermissionType permissionTypeRequest,
                int expirationDays)) = command;

        BudgetPermissionRequestId budgetPermissionRequestId =
            await _assignParticipantDomainService.AssignParticipantAsync(budgetId, participantId,
                AssignParticipantRequestMap.ToPermissionType(permissionTypeRequest), expirationDays, cancellationToken);

        return new AssignParticipantResponse(budgetPermissionRequestId.Value);
    }
}
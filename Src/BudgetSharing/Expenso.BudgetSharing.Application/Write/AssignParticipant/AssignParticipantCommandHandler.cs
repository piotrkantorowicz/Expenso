using Expenso.BudgetSharing.Application.Write.AssignParticipant.DTO.Reponses;
using Expenso.BudgetSharing.Application.Write.AssignParticipant.DTO.Requests;
using Expenso.BudgetSharing.Application.Write.AssignParticipant.DTO.Requests.Maps;
using Expenso.BudgetSharing.Domain.BudgetPermissionRequests.Services;
using Expenso.BudgetSharing.Domain.BudgetPermissionRequests.ValueObjects;
using Expenso.Shared.Commands;

namespace Expenso.BudgetSharing.Application.Write.AssignParticipant;

internal sealed class AssignParticipantCommandHandler(IAssignParticipantDomainService assignParticipantDomainService)
    : ICommandHandler<AssignParticipantCommand, AssignParticipantResponse>
{
    private readonly IAssignParticipantDomainService _assignParticipantDomainService = assignParticipantDomainService ??
        throw new ArgumentNullException(nameof(assignParticipantDomainService));

    public async Task<AssignParticipantResponse?> HandleAsync(AssignParticipantCommand command,
        CancellationToken cancellationToken = default)
    {
        (Guid budgetPermissionId, Guid participantId, AssignParticipantRequestPermissionType permissionTypeRequest,
            int expirationDays) = command.AssignParticipantRequest;

        BudgetPermissionRequestId budgetPermissionRequestId =
            await _assignParticipantDomainService.AssignParticipantAsync(budgetPermissionId, participantId,
                AssignParticipantRequestMap.ToPermissionType(permissionTypeRequest), expirationDays, cancellationToken);

        return new AssignParticipantResponse(budgetPermissionRequestId.Value);
    }
}
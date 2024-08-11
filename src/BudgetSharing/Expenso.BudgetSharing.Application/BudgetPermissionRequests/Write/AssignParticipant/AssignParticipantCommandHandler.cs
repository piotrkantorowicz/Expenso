using Expenso.BudgetSharing.Application.BudgetPermissionRequests.Write.AssignParticipant.DTO.Request;
using Expenso.BudgetSharing.Application.BudgetPermissionRequests.Write.AssignParticipant.DTO.Request.Maps;
using Expenso.BudgetSharing.Application.BudgetPermissionRequests.Write.AssignParticipant.DTO.Response;
using Expenso.BudgetSharing.Domain.BudgetPermissionRequests;
using Expenso.BudgetSharing.Domain.BudgetPermissionRequests.Services.Interfaces;
using Expenso.Shared.Commands;

namespace Expenso.BudgetSharing.Application.BudgetPermissionRequests.Write.AssignParticipant;

internal sealed class AssignParticipantCommandHandler(IAssignParticipantDomainService assignParticipantDomainService)
    : ICommandHandler<AssignParticipantCommand, AssignParticipantResponse>
{
    private readonly IAssignParticipantDomainService _assignParticipantDomainService = assignParticipantDomainService ??
        throw new ArgumentNullException(paramName: nameof(assignParticipantDomainService));

    public async Task<AssignParticipantResponse?> HandleAsync(AssignParticipantCommand command,
        CancellationToken cancellationToken)
    {
        (_,
            (Guid budgetId, string email, AssignParticipantRequest_PermissionType permissionTypeRequest,
                int expirationDays)) = command;

        BudgetPermissionRequest budgetPermissionRequest = await _assignParticipantDomainService.AssignParticipantAsync(
            budgetId: budgetId, email: email,
            permissionType: AssignParticipantRequestMap.ToPermissionType(
                assignParticipantRequestPermissionType: permissionTypeRequest), expirationDays: expirationDays,
            cancellationToken: cancellationToken);

        return new AssignParticipantResponse(BudgetPermissionRequestId: budgetPermissionRequest.Id.Value);
    }
}
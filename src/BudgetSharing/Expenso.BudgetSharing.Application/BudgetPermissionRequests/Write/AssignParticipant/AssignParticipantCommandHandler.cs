using Expenso.BudgetSharing.Application.BudgetPermissionRequests.Write.AssignParticipant.DTO.Request;
using Expenso.BudgetSharing.Application.BudgetPermissionRequests.Write.AssignParticipant.DTO.Request.Maps;
using Expenso.BudgetSharing.Application.BudgetPermissionRequests.Write.AssignParticipant.DTO.Response;
using Expenso.BudgetSharing.Domain.BudgetPermissionRequests;
using Expenso.BudgetSharing.Domain.BudgetPermissionRequests.Services.Interfaces;
using Expenso.BudgetSharing.Domain.Shared.ValueObjects;
using Expenso.Shared.Commands;

namespace Expenso.BudgetSharing.Application.BudgetPermissionRequests.Write.AssignParticipant;

internal sealed class
    AssignParticipantCommandHandler : ICommandHandler<AssignParticipantCommand, AssignParticipantResponse>
{
    private readonly IAssignParticipantionDomainService _assignParticipantionDomainService;

    public AssignParticipantCommandHandler(IAssignParticipantionDomainService assignParticipantionDomainService)
    {
        _assignParticipantionDomainService = assignParticipantionDomainService ??
                                             throw new ArgumentNullException(
                                                 paramName: nameof(assignParticipantionDomainService));
    }

    public async Task<AssignParticipantResponse?> HandleAsync(AssignParticipantCommand command,
        CancellationToken cancellationToken)
    {
        (_,
            (Guid budgetId, string email, AssignParticipantRequest_PermissionType permissionTypeRequest,
                int expirationDays)) = command;

        BudgetPermissionRequest budgetPermissionRequest =
            await _assignParticipantionDomainService.AssignParticipantAsync(budgetId: BudgetId.New(value: budgetId),
                email: email,
                permissionType: AssignParticipantRequestMap.ToPermissionType(
                    assignParticipantRequestPermissionType: permissionTypeRequest), expirationDays: expirationDays,
                cancellationToken: cancellationToken);

        return new AssignParticipantResponse(BudgetPermissionRequestId: budgetPermissionRequest.Id.Value);
    }
}
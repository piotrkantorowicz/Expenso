using Expenso.BudgetSharing.Application.DTO.AssignParticipant;
using Expenso.BudgetSharing.Application.Mappings;
using Expenso.BudgetSharing.Domain.BudgetPermissionRequests.Model;
using Expenso.BudgetSharing.Domain.BudgetPermissionRequests.Repositories;
using Expenso.IAM.Proxy;
using Expenso.IAM.Proxy.DTO.GetUser;
using Expenso.Shared.Commands;
using Expenso.Shared.Types.Exceptions;

namespace Expenso.BudgetSharing.Application.Commands.AssignParticipant;

internal sealed class AssignParticipantCommandHandler(
    IBudgetPermissionRequestRepository budgetPermissionRequestRepository,
    IIamProxy iamProxy) : ICommandHandler<AssignParticipantCommand>
{
    private readonly IBudgetPermissionRequestRepository _budgetPermissionRequestRepository =
        budgetPermissionRequestRepository ?? throw new ArgumentNullException(nameof(budgetPermissionRequestRepository));

    private readonly IIamProxy _iamProxy = iamProxy ?? throw new ArgumentNullException(nameof(iamProxy));

    public async Task HandleAsync(AssignParticipantCommand command, CancellationToken cancellationToken = default)
    {
        (Guid budgetId, Guid participantId, PermissionTypeRequest permissionTypeRequest) = command.AssignParticipant;
        GetUserInternalResponse? user = await _iamProxy.GetUserByIdAsync(participantId.ToString());

        if (user is null)
        {
            throw new NotFoundException($"User with id {participantId} hasn't been found");
        }

        BudgetPermissionRequest budgetPermissionRequest = BudgetPermissionRequest.Create(budgetId, participantId,
            PermissionTypeMap.ToPermissionType(permissionTypeRequest), null);

        await _budgetPermissionRequestRepository.AddAsync(budgetPermissionRequest, cancellationToken);
    }
}

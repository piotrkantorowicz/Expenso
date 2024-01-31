using Expenso.BudgetSharing.Application.DTO.AssignParticipant.Reponses;
using Expenso.BudgetSharing.Application.DTO.AssignParticipant.Requests;
using Expenso.BudgetSharing.Application.DTO.AssignParticipant.Requests.Maps;
using Expenso.BudgetSharing.Domain.BudgetPermissionRequests.Model;
using Expenso.BudgetSharing.Domain.BudgetPermissionRequests.Repositories;
using Expenso.IAM.Proxy;
using Expenso.IAM.Proxy.DTO.GetUser;
using Expenso.Shared.Commands;
using Expenso.Shared.Types.Clock;
using Expenso.Shared.Types.Exceptions;

namespace Expenso.BudgetSharing.Application.Commands.AssignParticipant;

internal sealed class AssignParticipantCommandHandler(
    IBudgetPermissionRequestRepository budgetPermissionRequestRepository,
    IIamProxy iamProxy,
    IClock clock) : ICommandHandler<AssignParticipantCommand, AssignParticipantResponse>
{
    private readonly IBudgetPermissionRequestRepository _budgetPermissionRequestRepository =
        budgetPermissionRequestRepository ?? throw new ArgumentNullException(nameof(budgetPermissionRequestRepository));

    private readonly IClock _clock = clock ?? throw new ArgumentNullException(nameof(clock));
    private readonly IIamProxy _iamProxy = iamProxy ?? throw new ArgumentNullException(nameof(iamProxy));

    public async Task<AssignParticipantResponse?> HandleAsync(AssignParticipantCommand command,
        CancellationToken cancellationToken = default)
    {
        (Guid budgetPermissionId, Guid participantId, AssignParticipantRequestPermissionType permissionTypeRequest,
            int expirationDays) = command.AssignParticipantRequest;

        GetUserInternalResponse? user = await _iamProxy.GetUserByIdAsync(participantId.ToString(), cancellationToken);

        if (user is null)
        {
            throw new NotFoundException($"User with id {participantId} hasn't been found");
        }

        BudgetPermissionRequest budgetPermissionRequest = BudgetPermissionRequest.Create(budgetPermissionId,
            participantId, AssignParticipantRequestMap.ToPermissionType(permissionTypeRequest), expirationDays, _clock);

        await _budgetPermissionRequestRepository.AddAsync(budgetPermissionRequest, cancellationToken);

        return new AssignParticipantResponse(budgetPermissionRequest.Id);
    }
}
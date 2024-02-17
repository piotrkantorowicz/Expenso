using Expenso.BudgetSharing.Domain.BudgetPermissionRequests.Repositories;
using Expenso.BudgetSharing.Domain.BudgetPermissionRequests.ValueObjects;
using Expenso.BudgetSharing.Domain.Shared.Model.ValueObjects;
using Expenso.IAM.Proxy;
using Expenso.Shared.System.Types.Clock;

namespace Expenso.BudgetSharing.Domain.BudgetPermissionRequests.Services;

internal sealed class AssignParticipantDomainService(
    IIamProxy iamProxy,
    IBudgetPermissionRequestRepository budgetPermissionRequestRepository,
    IClock clock) : IAssignParticipantDomainService
{
    private readonly IBudgetPermissionRequestRepository _budgetPermissionRequestRepository =
        budgetPermissionRequestRepository ?? throw new ArgumentNullException(nameof(budgetPermissionRequestRepository));

    private readonly IClock _clock = clock ?? throw new ArgumentNullException(nameof(clock));
    private readonly IIamProxy _iamProxy = iamProxy ?? throw new ArgumentNullException(nameof(iamProxy));

    public async Task<BudgetPermissionRequestId> AssignParticipantAsync(Guid budgetPermissionId, Guid participantId,
        PermissionType permissionType, int expirationDays, CancellationToken cancellationToken)
    {
        // GetUserInternalResponse? user = await _iamProxy.GetUserByIdAsync(participantId.ToString(), cancellationToken);
        //
        // DomainModelState.CheckBusinessRules(
        //     [new OnlyExistingUserCanBeAssignedAsBudgetParticipant(participantId, user)], false);

        BudgetPermissionRequest budgetPermissionRequest = BudgetPermissionRequest.Create(
            BudgetId.New(budgetPermissionId), PersonId.New(new Guid("e253a2f0-e47f-47a3-9d65-f3468f32a5d5")),
            permissionType, expirationDays, _clock);

        await _budgetPermissionRequestRepository.AddAsync(budgetPermissionRequest, cancellationToken);

        return budgetPermissionRequest.Id;
    }
}
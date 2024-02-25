using Expenso.BudgetSharing.Domain.BudgetPermissionRequests.Repositories;
using Expenso.BudgetSharing.Domain.BudgetPermissionRequests.Rules;
using Expenso.BudgetSharing.Domain.BudgetPermissionRequests.Services.Interfaces;
using Expenso.BudgetSharing.Domain.BudgetPermissionRequests.ValueObjects;
using Expenso.BudgetSharing.Domain.Shared.Model.ValueObjects;
using Expenso.IAM.Proxy;
using Expenso.IAM.Proxy.DTO.GetUser;
using Expenso.Shared.Domain.Types.Model;
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

    public async Task<BudgetPermissionRequestId> AssignParticipantAsync(Guid budgetId, Guid participantId,
        PermissionType permissionType, int expirationDays, CancellationToken cancellationToken)
    {
        GetUserExternalResponse? user = await _iamProxy.GetUserByIdAsync(participantId.ToString(), cancellationToken);

        DomainModelState.CheckBusinessRules([new OnlyExistingUserCanBeAssignedAsBudgetParticipant(participantId, user)],
            false);

        BudgetPermissionRequest budgetPermissionRequest = BudgetPermissionRequest.Create(BudgetId.New(budgetId),
            PersonId.New(participantId), permissionType, expirationDays, _clock);

        await _budgetPermissionRequestRepository.AddAsync(budgetPermissionRequest, cancellationToken);

        return budgetPermissionRequest.Id;
    }
}
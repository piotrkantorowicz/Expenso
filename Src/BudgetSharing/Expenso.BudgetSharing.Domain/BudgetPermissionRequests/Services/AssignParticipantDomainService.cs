using Expenso.BudgetSharing.Domain.BudgetPermissionRequests.Repositories;
using Expenso.BudgetSharing.Domain.BudgetPermissionRequests.Rules;
using Expenso.BudgetSharing.Domain.BudgetPermissionRequests.Services.Interfaces;
using Expenso.BudgetSharing.Domain.Shared.Model.ValueObjects;
using Expenso.IAM.Core.Users.Queries.GetUser.DTO.Response;
using Expenso.IAM.Proxy;
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

    public async Task<BudgetPermissionRequest> AssignParticipantAsync(Guid budgetId, string email,
        PermissionType permissionType, int expirationDays, CancellationToken cancellationToken)
    {
        GetUserResponse? user = await _iamProxy.GetUserByEmailAsync(email, cancellationToken);
        DomainModelState.CheckBusinessRules([new OnlyExistingUserCanBeAssignedAsBudgetParticipant(email, user)]);
        PersonId participantId = PersonId.New(Guid.Parse(user!.UserId));

        BudgetPermissionRequest budgetPermissionRequest = BudgetPermissionRequest.Create(BudgetId.New(budgetId),
            participantId, permissionType, expirationDays, _clock);

        await _budgetPermissionRequestRepository.AddAsync(budgetPermissionRequest, cancellationToken);

        return budgetPermissionRequest;
    }
}
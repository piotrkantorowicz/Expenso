using Expenso.BudgetSharing.Domain.BudgetPermissionRequests.Repositories;
using Expenso.BudgetSharing.Domain.BudgetPermissionRequests.Rules;
using Expenso.BudgetSharing.Domain.BudgetPermissionRequests.Services.Interfaces;
using Expenso.BudgetSharing.Domain.Shared.ValueObjects;
using Expenso.IAM.Proxy;
using Expenso.IAM.Proxy.DTO.GetUser;
using Expenso.Shared.Domain.Types.Model;
using Expenso.Shared.System.Types.Clock;

namespace Expenso.BudgetSharing.Domain.BudgetPermissionRequests.Services;

internal sealed class AssignParticipantDomainService : IAssignParticipantDomainService
{
    private readonly IBudgetPermissionRequestRepository _budgetPermissionRequestRepository;
    private readonly IClock _clock;
    private readonly IIamProxy _iamProxy;

    public AssignParticipantDomainService(IIamProxy iamProxy,
        IBudgetPermissionRequestRepository budgetPermissionRequestRepository, IClock clock)
    {
        _budgetPermissionRequestRepository = budgetPermissionRequestRepository ??
                                             throw new ArgumentNullException(
                                                 paramName: nameof(budgetPermissionRequestRepository));

        _clock = clock ?? throw new ArgumentNullException(paramName: nameof(clock));
        _iamProxy = iamProxy ?? throw new ArgumentNullException(paramName: nameof(iamProxy));
    }

    public async Task<BudgetPermissionRequest> AssignParticipantAsync(Guid budgetId, string email,
        PermissionType permissionType, int expirationDays, CancellationToken cancellationToken)
    {
        GetUserResponse? user = await _iamProxy.GetUserByEmailAsync(email: email, cancellationToken: cancellationToken);

        DomainModelState.CheckBusinessRules(businessRules:
        [
            (new OnlyExistingUserCanBeAssignedAsBudgetParticipant(email: email, user: user), false)
        ]);

        PersonId participantId = PersonId.New(value: Guid.Parse(input: user!.UserId));

        BudgetPermissionRequest budgetPermissionRequest = BudgetPermissionRequest.Create(
            budgetId: BudgetId.New(value: budgetId), personId: participantId, permissionType: permissionType,
            expirationDays: expirationDays, clock: _clock);

        await _budgetPermissionRequestRepository.AddAsync(permission: budgetPermissionRequest,
            cancellationToken: cancellationToken);

        return budgetPermissionRequest;
    }
}
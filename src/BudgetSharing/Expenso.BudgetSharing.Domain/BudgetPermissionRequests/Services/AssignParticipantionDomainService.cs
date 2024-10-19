using Expenso.BudgetSharing.Domain.BudgetPermissionRequests.Repositories;
using Expenso.BudgetSharing.Domain.BudgetPermissionRequests.Rules;
using Expenso.BudgetSharing.Domain.BudgetPermissionRequests.Services.Interfaces;
using Expenso.BudgetSharing.Domain.BudgetPermissions;
using Expenso.BudgetSharing.Domain.BudgetPermissions.Repositories;
using Expenso.BudgetSharing.Domain.Shared.ValueObjects;
using Expenso.IAM.Shared;
using Expenso.IAM.Shared.DTO.GetUser.Response;
using Expenso.Shared.Domain.Types.Model;
using Expenso.Shared.Domain.Types.Rules;
using Expenso.Shared.System.Types.Clock;

namespace Expenso.BudgetSharing.Domain.BudgetPermissionRequests.Services;

internal sealed class AssignParticipantionDomainService : IAssignParticipantionDomainService
{
    private readonly IBudgetPermissionRepository _budgetPermissionRepository;
    private readonly IBudgetPermissionRequestRepository _budgetPermissionRequestRepository;
    private readonly IClock _clock;
    private readonly IIamProxy _iamProxy;

    public AssignParticipantionDomainService(IIamProxy iamProxy,
        IBudgetPermissionRequestRepository budgetPermissionRequestRepository, IClock clock,
        IBudgetPermissionRepository budgetPermissionRepository)
    {
        _budgetPermissionRequestRepository = budgetPermissionRequestRepository ??
                                             throw new ArgumentNullException(
                                                 paramName: nameof(budgetPermissionRequestRepository));

        _clock = clock ?? throw new ArgumentNullException(paramName: nameof(clock));

        _budgetPermissionRepository = budgetPermissionRepository ??
                                      throw new ArgumentNullException(paramName: nameof(budgetPermissionRepository));

        _iamProxy = iamProxy ?? throw new ArgumentNullException(paramName: nameof(iamProxy));
    }

    public async Task<BudgetPermissionRequest> AssignParticipantAsync(BudgetId budgetId, string email,
        PermissionType permissionType, int expirationDays, CancellationToken cancellationToken)
    {
        BudgetPermission? budgetPermission =
            await _budgetPermissionRepository.GetByBudgetIdAsync(budgetId: budgetId,
                cancellationToken: cancellationToken);

        DomainModelState.CheckBusinessRules(businessRules:
        [
            new BusinesRuleCheck(
                BusinessRule: new BudgetPermissionMustExistsToBeAbleToCreateBudgetPermissionRequest(budgetId: budgetId,
                    budgetPermission: budgetPermission), ThrowException: true)
        ]);

        GetUserResponse? user = await _iamProxy.GetUserByEmailAsync(email: email, cancellationToken: cancellationToken);

        DomainModelState.CheckBusinessRules(businessRules:
        [
            new BusinesRuleCheck(
                BusinessRule: new OnlyExistingUserCanBeAssignedAsBudgetParticipant(email: email, user: user),
                ThrowException: true)
        ]);

        PersonId participantId = PersonId.New(value: Guid.Parse(input: user!.UserId));

        DomainModelState.CheckBusinessRules(businessRules:
        [
            new BusinesRuleCheck(
                BusinessRule: new MemberHasAlreadyAssignedToRequestedBudget(participantId: participantId,
                    budgetPermission: budgetPermission), ThrowException: true)
        ]);

        IReadOnlyCollection<BudgetPermissionRequest> budgetPermissionRequests =
            await _budgetPermissionRequestRepository.GetUncompletedByPersonIdAsync(budgetId: budgetId,
                participantId: participantId, cancellationToken: cancellationToken);

        DomainModelState.CheckBusinessRules(businessRules:
        [
            new BusinesRuleCheck(
                BusinessRule: new MemberHasAlreadyOpenedBudgetPermissionRequests(participantId: participantId,
                    budgetId: budgetId, permissionType: permissionType,
                    budgetPermissionRequests: budgetPermissionRequests), ThrowException: true)
        ]);

        BudgetPermissionRequest budgetPermissionRequest = BudgetPermissionRequest.Create(budgetId: budgetId,
            ownerId: budgetPermission!.OwnerId, personId: participantId, permissionType: permissionType,
            expirationDays: expirationDays, clock: _clock);

        await _budgetPermissionRequestRepository.AddAsync(permission: budgetPermissionRequest,
            cancellationToken: cancellationToken);

        return budgetPermissionRequest;
    }
}
using Expenso.BudgetSharing.Domain.BudgetPermissionRequests.Repositories;
using Expenso.BudgetSharing.Domain.BudgetPermissionRequests.Rules;
using Expenso.BudgetSharing.Domain.BudgetPermissionRequests.Services.Interfaces;
using Expenso.BudgetSharing.Domain.BudgetPermissions;
using Expenso.BudgetSharing.Domain.BudgetPermissions.Repositories;
using Expenso.BudgetSharing.Domain.Shared.Rules;
using Expenso.BudgetSharing.Domain.Shared.ValueObjects;
using Expenso.IAM.Shared;
using Expenso.IAM.Shared.DTO.GetUserByEmail.Response;
using Expenso.Shared.Domain.Types.Model;
using Expenso.Shared.Domain.Types.Rules;
using Expenso.Shared.Domain.Types.ValueObjects;
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

    public async Task<BudgetPermissionRequest> AssignParticipantAsync(BudgetId budgetId, string? email,
        PermissionType? permissionType, int expirationDays, CancellationToken cancellationToken)
    {
        BudgetPermission? budgetPermission =
            await _budgetPermissionRepository.GetByBudgetIdAsync(budgetId: budgetId,
                cancellationToken: cancellationToken);

        DomainModelState.CheckBusinessRules(businessRules:
        [
            new BusinessRuleCheck(
                BusinessRule: new BudgetPermissionMustExistsToBeAbleToCreateBudgetPermissionRequest(budgetId: budgetId,
                    budgetPermission: budgetPermission)),
            new BusinessRuleCheck(
                BusinessRule: new ParticipantEmailShouldBeAValidEmailAddress(email: email, budgetId: budgetId))
        ]);

        GetUserByEmailResponse? user =
            await _iamProxy.GetUserByEmailAsync(email: email!, cancellationToken: cancellationToken);

        DomainModelState.CheckBusinessRules(businessRules:
        [
            new BusinessRuleCheck(
                BusinessRule: new OnlyExistingUserCanBeAssignedAsBudgetParticipant(email: email!, userId: user?.UserId))
        ]);

        PersonId participantId = PersonId.New(value: Guid.Parse(input: user!.UserId));

        DomainModelState.CheckBusinessRules(businessRules:
        [
            new BusinessRuleCheck(
                BusinessRule: new MemberHasAlreadyAssignedToRequestedBudget(participantId: participantId,
                    budgetPermission: budgetPermission), ThrowException: true)
        ]);

        IReadOnlyCollection<BudgetPermissionRequest> budgetPermissionRequests =
            await _budgetPermissionRequestRepository.GetUncompletedByPersonIdAsync(budgetId: budgetId,
                participantId: participantId, cancellationToken: cancellationToken);

        DateAndTime submissionDate = DateAndTime.New(value: _clock.UtcNow);
        DateAndTime expirationDate = DateAndTime.New(value: submissionDate.Value.AddDays(days: expirationDays));

        DomainModelState.CheckBusinessRules(businessRules:
        [
            new BusinessRuleCheck(
                BusinessRule: new ParticipantPermissionTypeMustHaveValue(permissionType: permissionType,
                    participantId: participantId))
        ]);

        DomainModelState.CheckBusinessRules(businessRules:
        [
            new BusinessRuleCheck(
                BusinessRule: new ExpirationDateMustBeGreaterThanOneDay(expirationDate: expirationDate, clock: _clock)),
            new BusinessRuleCheck(BusinessRule: new MemberHasAlreadyOpenedBudgetPermissionRequests(
                participantId: participantId, budgetId: budgetId, permissionType: permissionType!,
                budgetPermissionRequests: budgetPermissionRequests))
        ]);

        BudgetPermissionRequest budgetPermissionRequest = BudgetPermissionRequest.Create(budgetId: budgetId,
            ownerId: budgetPermission!.OwnerId, personId: participantId, permissionType: permissionType!,
            expirationDate: expirationDate, submissionDate: submissionDate);

        await _budgetPermissionRequestRepository.AddAsync(permission: budgetPermissionRequest,
            cancellationToken: cancellationToken);

        return budgetPermissionRequest;
    }
}
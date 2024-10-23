using Expenso.BudgetSharing.Domain.BudgetPermissionRequests.Rules;
using Expenso.Shared.Domain.Types.Model;
using Expenso.Shared.Domain.Types.Rules;
using Expenso.Shared.Domain.Types.ValueObjects;
using Expenso.Shared.System.Types.Clock;

namespace Expenso.BudgetSharing.Domain.BudgetPermissionRequests.ValueObjects;

public sealed record BudgetPermissionRequestStatusTracker
{
    private BudgetPermissionRequestStatusTracker()
    {
        BudgetPermissionRequestId = default!;
        ExpirationDate = default!;
        SubmissionDate = default!;
        ConfirmationDate = default!;
        CancellationDate = default!;
        Status = default!;
    }

    private BudgetPermissionRequestStatusTracker(BudgetPermissionRequestId budgetPermissionRequestId, IClock clock,
        DateAndTime expirationDate, BudgetPermissionRequestStatus status)
    {
        DateAndTime submissionDate = DateAndTime.New(value: clock.UtcNow);

        DomainModelState.CheckBusinessRules(businessRules:
        [
            new BusinessRuleCheck(
                BusinessRule: new BudgetPermissionRequestStatusMustBePendingAtTheBeginingRule(status: status)),
            new BusinessRuleCheck(
                BusinessRule: new ExpirationDateMustBeGreaterThanSubmissionDate(expirationDate: expirationDate,
                    submissionDate: submissionDate))
        ]);

        BudgetPermissionRequestId = budgetPermissionRequestId;
        ExpirationDate = expirationDate;
        SubmissionDate = submissionDate;
        Status = status;
    }

    public BudgetPermissionRequestId BudgetPermissionRequestId { get; }

    public DateAndTime ExpirationDate { get; }

    public DateAndTime SubmissionDate { get; }

    public DateAndTime? ConfirmationDate { get; private set; }

    public DateAndTime? CancellationDate { get; private set; }

    public BudgetPermissionRequestStatus Status { get; private set; }

    public static BudgetPermissionRequestStatusTracker Start(BudgetPermissionRequestId budgetPermissionRequestId,
        IClock clock, DateAndTime expirationDate, BudgetPermissionRequestStatus status)
    {
        return new BudgetPermissionRequestStatusTracker(budgetPermissionRequestId: budgetPermissionRequestId,
            clock: clock, expirationDate: expirationDate, status: status);
    }

    public void Cancel(IClock clock)
    {
        DomainModelState.CheckBusinessRules(businessRules:
        [
            new BusinessRuleCheck(
                BusinessRule: new OnlyPendingBudgetPermissionRequestCanBeMadeCancelled(
                    budgetPermissionRequestId: BudgetPermissionRequestId, status: Status))
        ]);

        CancellationDate = DateAndTime.New(value: clock.UtcNow);
        Status = BudgetPermissionRequestStatus.Cancelled;
    }

    public void Confirm(IClock clock)
    {
        DomainModelState.CheckBusinessRules(businessRules:
        [
            new BusinessRuleCheck(
                BusinessRule: new OnlyPendingBudgetPermissionRequestCanBeMadeConfirmed(
                    budgetPermissionRequestId: BudgetPermissionRequestId, status: Status))
        ]);

        ConfirmationDate = DateAndTime.New(value: clock.UtcNow);
        Status = BudgetPermissionRequestStatus.Confirmed;
    }

    public void Expire()
    {
        DomainModelState.CheckBusinessRules(businessRules:
        [
            new BusinessRuleCheck(
                BusinessRule: new OnlyPendingBudgetPermissionRequestCanBeMadeExpired(
                    budgetPermissionRequestId: BudgetPermissionRequestId, status: Status))
        ]);

        Status = BudgetPermissionRequestStatus.Expired;
    }
}
using Expenso.BudgetSharing.Domain.BudgetPermissionRequests.Rules;
using Expenso.Shared.Domain.Types.Model;
using Expenso.Shared.Domain.Types.Rules;
using Expenso.Shared.Domain.Types.ValueObjects;

namespace Expenso.BudgetSharing.Domain.BudgetPermissionRequests.ValueObjects;

public sealed record BudgetPermissionRequestStatusTracker
{
    // ReSharper disable once UnusedMember.Local
    // Required for EF Core 
    private BudgetPermissionRequestStatusTracker()
    {
        BudgetPermissionRequestId = default!;
        ExpirationDate = default!;
        SubmissionDate = default!;
        ConfirmationDate = default!;
        CancellationDate = default!;
        Status = default!;
    }

    private BudgetPermissionRequestStatusTracker(BudgetPermissionRequestId budgetPermissionRequestId,
        DateAndTime submissionDate, DateAndTime expirationDate, BudgetPermissionRequestStatus status)
    {
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
        DateAndTime submissionDate, DateAndTime expirationDate, BudgetPermissionRequestStatus status)
    {
        return new BudgetPermissionRequestStatusTracker(budgetPermissionRequestId: budgetPermissionRequestId,
            submissionDate: submissionDate, expirationDate: expirationDate, status: status);
    }

    public void Cancel(DateAndTime cancellationDate)
    {
        DomainModelState.CheckBusinessRules(businessRules:
        [
            new BusinessRuleCheck(
                BusinessRule: new OnlyPendingBudgetPermissionRequestCanBeMadeCancelled(
                    budgetPermissionRequestId: BudgetPermissionRequestId, status: Status)),
            new BusinessRuleCheck(
                BusinessRule: new CancellationDateMustBeGreaterThanSubmissionDate(cancellationDate: cancellationDate,
                    submissionDate: SubmissionDate))
        ]);

        CancellationDate = cancellationDate;
        Status = BudgetPermissionRequestStatus.Cancelled;
    }

    public void Confirm(DateAndTime confirmationDate)
    {
        DomainModelState.CheckBusinessRules(businessRules:
        [
            new BusinessRuleCheck(
                BusinessRule: new OnlyPendingBudgetPermissionRequestCanBeMadeConfirmed(
                    budgetPermissionRequestId: BudgetPermissionRequestId, status: Status)),
            new BusinessRuleCheck(
                BusinessRule: new ConfirmationDateMustBeGreaterThanSubmissionDate(confirmationDate: confirmationDate,
                    submissionDate: SubmissionDate))
        ]);

        ConfirmationDate = confirmationDate;
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
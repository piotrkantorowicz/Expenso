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
        ExpirationDate = default!;
        SubmissionDate = default!;
        ConfirmationDate = default!;
        CancellationDate = default!;
        Status = default!;
    }

    private BudgetPermissionRequestStatusTracker(IClock clock, DateAndTime expirationDate,
        BudgetPermissionRequestStatus status)
    {
        DateAndTime submissionDate = DateAndTime.New(value: clock.UtcNow);

        DomainModelState.CheckBusinessRules(businessRules:
        [
            new BusinesRuleCheck(
                BusinessRule: new UnknownBudgetPermissionRequestStatusCannotBeProcessed(status: status)),
            new BusinesRuleCheck(
                BusinessRule: new ExpirationDateMustBeGreaterThanSubmissionDate(expirationDate: expirationDate,
                    submissionDate: submissionDate))
        ]);

        ExpirationDate = expirationDate;
        SubmissionDate = submissionDate;
        Status = BudgetPermissionRequestStatus.Pending;
    }

    public DateAndTime ExpirationDate { get; }

    public DateAndTime SubmissionDate { get; }

    public DateAndTime? ConfirmationDate { get; private set; }

    public DateAndTime? CancellationDate { get; private set; }

    public BudgetPermissionRequestStatus Status { get; private set; }

    public static BudgetPermissionRequestStatusTracker Start(IClock clock, DateAndTime expirationDate,
        BudgetPermissionRequestStatus status)
    {
        return new BudgetPermissionRequestStatusTracker(clock: clock, expirationDate: expirationDate, status: status);
    }

    public void Cancel(IClock clock)
    {
        CancellationDate = DateAndTime.New(value: clock.UtcNow);
        Status = BudgetPermissionRequestStatus.Cancelled;
    }

    public void Confirm(IClock clock)
    {
        ConfirmationDate = DateAndTime.New(value: clock.UtcNow);
        Status = BudgetPermissionRequestStatus.Confirmed;
    }

    public void Expire()
    {
        Status = BudgetPermissionRequestStatus.Expired;
    }
}
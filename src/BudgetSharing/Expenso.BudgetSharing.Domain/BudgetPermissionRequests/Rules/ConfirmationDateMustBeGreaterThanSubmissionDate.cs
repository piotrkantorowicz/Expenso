using Expenso.Shared.Domain.Types.Rules;
using Expenso.Shared.Domain.Types.ValueObjects;

namespace Expenso.BudgetSharing.Domain.BudgetPermissionRequests.Rules;

internal sealed class ConfirmationDateMustBeGreaterThanSubmissionDate : IBusinessRule
{
    private readonly DateAndTime _confirmationDate;
    private readonly DateAndTime _submissionDate;

    public ConfirmationDateMustBeGreaterThanSubmissionDate(DateAndTime submissionDate, DateAndTime confirmationDate)
    {
        _submissionDate = submissionDate ?? throw new ArgumentNullException(paramName: nameof(submissionDate));
        _confirmationDate = confirmationDate ?? throw new ArgumentNullException(paramName: nameof(confirmationDate));
    }

    public string Message =>
        $"Confirmation date {_confirmationDate} must be greater than submission date: {_submissionDate}.";

    public bool IsBroken()
    {
        return _confirmationDate.LessThanOrEqual(dateTimeOffset: _submissionDate);
    }
}
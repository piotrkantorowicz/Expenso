using Expenso.Shared.Domain.Types.Rules;
using Expenso.Shared.Domain.Types.ValueObjects;

namespace Expenso.BudgetSharing.Domain.BudgetPermissionRequests.Rules;

internal sealed class CancellationDateMustBeGreaterThanSubmissionDate : IBusinessRule
{
    private readonly DateAndTime _cancellationDate;
    private readonly DateAndTime _submissionDate;

    public CancellationDateMustBeGreaterThanSubmissionDate(DateAndTime submissionDate, DateAndTime cancellationDate)
    {
        _submissionDate = submissionDate ?? throw new ArgumentNullException(paramName: nameof(submissionDate));
        _cancellationDate = cancellationDate ?? throw new ArgumentNullException(paramName: nameof(cancellationDate));
    }

    public string Message =>
        $"Cancellation date {_cancellationDate} must be greater than submission date: {_submissionDate}.";

    public bool IsBroken()
    {
        return _cancellationDate.LessThanOrEqual(dateTimeOffset: _submissionDate);
    }
}
using Expenso.Shared.Domain.Types.Rules;
using Expenso.Shared.Domain.Types.ValueObjects;

namespace Expenso.BudgetSharing.Domain.BudgetPermissionRequests.Rules;

internal sealed class ExpirationDateMustBeGreaterThanSubmissionDate : IBusinessRule
{
    private readonly DateAndTime _expirationDate;
    private readonly DateAndTime _submissionDate;

    public ExpirationDateMustBeGreaterThanSubmissionDate(DateAndTime submissionDate, DateAndTime expirationDate)
    {
        _submissionDate = submissionDate ?? throw new ArgumentNullException(paramName: nameof(submissionDate));
        _expirationDate = expirationDate ?? throw new ArgumentNullException(paramName: nameof(expirationDate));
    }

    public string Message =>
        $"Expiration date {_expirationDate.Value} must be greater than Submission date: {_submissionDate.Value}.";

    public bool IsBroken()
    {
        return _expirationDate.LessThanOrEqual(dateTimeOffset: _submissionDate);
    }
}
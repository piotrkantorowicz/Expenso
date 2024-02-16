namespace Expenso.UserPreferences.Core.Domain.Preferences.Model;

internal sealed record FinancePreference
{
    public Guid Id { get; init; }

    public Guid PreferenceId { get; init; }

    public int MaxNumberOfFinancePlanReviewers { get; init; }

    public bool AllowAddFinancePlanReviewers { get; init; }

    public int MaxNumberOfSubFinancePlanSubOwners { get; init; }

    public bool AllowAddFinancePlanSubOwners { get; init; }
}
namespace Expenso.UserPreferences.Core.Domain.Preferences.Model.ValueObjects;

internal sealed record FinancePreference
{
    private FinancePreference() : this(default, default, default, default)
    {
    }

    private FinancePreference(bool allowAddFinancePlanSubOwners, int maxNumberOfSubFinancePlanSubOwners,
        bool allowAddFinancePlanReviewers, int maxNumberOfFinancePlanReviewers)
    {
        AllowAddFinancePlanSubOwners = allowAddFinancePlanSubOwners;

        if (allowAddFinancePlanSubOwners)
        {
            MaxNumberOfSubFinancePlanSubOwners = maxNumberOfSubFinancePlanSubOwners;
        }

        AllowAddFinancePlanReviewers = allowAddFinancePlanReviewers;

        if (allowAddFinancePlanReviewers)
        {
            MaxNumberOfFinancePlanReviewers = maxNumberOfFinancePlanReviewers;
        }
    }

    public int MaxNumberOfFinancePlanReviewers { get; }

    public bool AllowAddFinancePlanReviewers { get; }

    public int MaxNumberOfSubFinancePlanSubOwners { get; }

    public bool AllowAddFinancePlanSubOwners { get; }

    public static FinancePreference CreateDefault()
    {
        return new FinancePreference();
    }

    public static FinancePreference Create(bool allowAddFinancePlanSubOwners, int maxNumberOfSubFinancePlanSubOwners,
        bool allowAddFinancePlanReviewers, int maxNumberOfFinancePlanReviewers)
    {
        return new FinancePreference(allowAddFinancePlanSubOwners, maxNumberOfSubFinancePlanSubOwners,
            allowAddFinancePlanReviewers, maxNumberOfFinancePlanReviewers);
    }
}
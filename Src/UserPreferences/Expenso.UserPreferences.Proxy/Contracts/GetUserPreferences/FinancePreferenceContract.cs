namespace Expenso.UserPreferences.Proxy.Contracts.GetUserPreferences;

public sealed record FinancePreferenceContract(
    bool AllowAddFinancePlanSubOwners,
    int MaxNumberOfSubFinancePlanSubOwners,
    bool AllowAddFinancePlanReviewers,
    int MaxNumberOfFinancePlanReviewers);
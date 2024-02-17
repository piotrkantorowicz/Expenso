namespace Expenso.UserPreferences.Proxy.DTO.MessageBus.FinancePreferences;

public sealed record FinancePreferenceContract(
    bool AllowAddFinancePlanSubOwners,
    int MaxNumberOfSubFinancePlanSubOwners,
    bool AllowAddFinancePlanReviewers,
    int MaxNumberOfFinancePlanReviewers);
namespace Expenso.UserPreferences.Proxy.DTO.MessageBus.FinancePreferences;

public sealed record FinancePreferenceInternalContract(
    bool AllowAddFinancePlanSubOwners,
    int MaxNumberOfSubFinancePlanSubOwners,
    bool AllowAddFinancePlanReviewers,
    int MaxNumberOfFinancePlanReviewers);
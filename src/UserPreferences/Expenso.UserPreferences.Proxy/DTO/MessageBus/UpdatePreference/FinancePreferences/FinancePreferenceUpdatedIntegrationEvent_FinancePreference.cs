namespace Expenso.UserPreferences.Proxy.DTO.MessageBus.UpdatePreference.FinancePreferences;

public sealed record FinancePreferenceUpdatedIntegrationEvent_FinancePreference(
    bool AllowAddFinancePlanSubOwners,
    int MaxNumberOfSubFinancePlanSubOwners,
    bool AllowAddFinancePlanReviewers,
    int MaxNumberOfFinancePlanReviewers);
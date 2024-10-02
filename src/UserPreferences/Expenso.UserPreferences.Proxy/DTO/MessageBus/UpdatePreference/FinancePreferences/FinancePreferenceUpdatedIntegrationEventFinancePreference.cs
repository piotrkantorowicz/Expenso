namespace Expenso.UserPreferences.Proxy.DTO.MessageBus.UpdatePreference.FinancePreferences;

public sealed record FinancePreferenceUpdatedIntegrationEventFinancePreference(
    bool AllowAddFinancePlanSubOwners,
    int MaxNumberOfSubFinancePlanSubOwners,
    bool AllowAddFinancePlanReviewers,
    int MaxNumberOfFinancePlanReviewers);
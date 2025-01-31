namespace Expenso.UserPreferences.Shared.DTO.API.UpdatePreference;

public sealed record UpdatePreferenceRequestFinancePreference(
    bool AllowAddFinancePlanSubOwners,
    int MaxNumberOfSubFinancePlanSubOwners,
    bool AllowAddFinancePlanReviewers,
    int MaxNumberOfFinancePlanReviewers);
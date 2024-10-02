namespace Expenso.UserPreferences.Proxy.DTO.API.GetPreference.Responses;

public sealed record GetPreferenceResponseFinancePreference(
    bool AllowAddFinancePlanSubOwners,
    int MaxNumberOfSubFinancePlanSubOwners,
    bool AllowAddFinancePlanReviewers,
    int MaxNumberOfFinancePlanReviewers);
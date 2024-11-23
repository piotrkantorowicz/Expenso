namespace Expenso.UserPreferences.Shared.DTO.API.GetPreference.Response;

public sealed record GetPreferencesResponseFinancePreference(
    bool AllowAddFinancePlanSubOwners,
    int MaxNumberOfSubFinancePlanSubOwners,
    bool AllowAddFinancePlanReviewers,
    int MaxNumberOfFinancePlanReviewers);
namespace Expenso.UserPreferences.Core.Application.Preferences.DTO.CreatePreference.Response;

public sealed record CreateFinancePreferenceResponse(
    bool AllowAddFinancePlanSubOwners,
    int MaxNumberOfSubFinancePlanSubOwners,
    bool AllowAddFinancePlanReviewers,
    int MaxNumberOfFinancePlanReviewers);
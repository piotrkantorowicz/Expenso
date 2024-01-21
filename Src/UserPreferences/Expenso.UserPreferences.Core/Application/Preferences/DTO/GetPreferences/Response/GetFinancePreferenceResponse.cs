namespace Expenso.UserPreferences.Core.Application.Preferences.DTO.GetPreferences.Response;

public sealed record GetFinancePreferenceResponse(
    bool AllowAddFinancePlanSubOwners,
    int MaxNumberOfSubFinancePlanSubOwners,
    bool AllowAddFinancePlanReviewers,
    int MaxNumberOfFinancePlanReviewers);
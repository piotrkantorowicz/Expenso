namespace Expenso.UserPreferences.Core.Application.Preferences.Read.Queries.GetPreference.DTO.Response;

public sealed record GetPreferenceResponseFinancePreference(
    bool AllowAddFinancePlanSubOwners,
    int MaxNumberOfSubFinancePlanSubOwners,
    bool AllowAddFinancePlanReviewers,
    int MaxNumberOfFinancePlanReviewers);
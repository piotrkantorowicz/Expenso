namespace Expenso.UserPreferences.Core.Application.Preferences.Read.Queries.GetPreferenceForCurrentUser.DTO.Response;

public sealed record GetPreferenceForCurrentUserResponseFinancePreference(
    bool AllowAddFinancePlanSubOwners,
    int MaxNumberOfSubFinancePlanSubOwners,
    bool AllowAddFinancePlanReviewers,
    int MaxNumberOfFinancePlanReviewers);
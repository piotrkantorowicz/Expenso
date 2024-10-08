namespace Expenso.UserPreferences.Shared.DTO.API.GetPreference.Response;

public sealed record GetPreferencesResponse_FinancePreference(
    bool AllowAddFinancePlanSubOwners,
    int MaxNumberOfSubFinancePlanSubOwners,
    bool AllowAddFinancePlanReviewers,
    int MaxNumberOfFinancePlanReviewers);
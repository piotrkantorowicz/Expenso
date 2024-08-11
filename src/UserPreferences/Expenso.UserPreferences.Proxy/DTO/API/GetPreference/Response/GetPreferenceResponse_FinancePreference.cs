namespace Expenso.UserPreferences.Proxy.DTO.API.GetPreference.Response;

public sealed record GetPreferenceResponse_FinancePreference(
    bool AllowAddFinancePlanSubOwners,
    int MaxNumberOfSubFinancePlanSubOwners,
    bool AllowAddFinancePlanReviewers,
    int MaxNumberOfFinancePlanReviewers);
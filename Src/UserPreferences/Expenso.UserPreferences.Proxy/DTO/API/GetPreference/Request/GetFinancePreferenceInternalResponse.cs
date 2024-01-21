namespace Expenso.UserPreferences.Proxy.DTO.API.GetPreference.Request;

public sealed record GetFinancePreferenceInternalResponse(
    bool AllowAddFinancePlanSubOwners,
    int MaxNumberOfSubFinancePlanSubOwners,
    bool AllowAddFinancePlanReviewers,
    int MaxNumberOfFinancePlanReviewers);
namespace Expenso.UserPreferences.Proxy.DTO.API.CreatePreference.Response;

public sealed record CreateFinancePreferenceResponse(
    bool AllowAddFinancePlanSubOwners,
    int MaxNumberOfSubFinancePlanSubOwners,
    bool AllowAddFinancePlanReviewers,
    int MaxNumberOfFinancePlanReviewers);
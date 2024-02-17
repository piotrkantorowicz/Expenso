namespace Expenso.UserPreferences.Core.Application.Preferences.Read.Queries.GetPreference.Internal.DTO.Response;

public sealed record GetFinancePreferenceResponse(
    bool AllowAddFinancePlanSubOwners,
    int MaxNumberOfSubFinancePlanSubOwners,
    bool AllowAddFinancePlanReviewers,
    int MaxNumberOfFinancePlanReviewers);
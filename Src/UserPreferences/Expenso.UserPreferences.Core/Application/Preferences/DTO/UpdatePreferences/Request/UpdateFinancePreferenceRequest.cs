namespace Expenso.UserPreferences.Core.Application.Preferences.DTO.UpdatePreferences.Request;

public sealed record UpdateFinancePreferenceRequest(
    bool AllowAddFinancePlanSubOwners,
    int MaxNumberOfSubFinancePlanSubOwners,
    bool AllowAddFinancePlanReviewers,
    int MaxNumberOfFinancePlanReviewers);
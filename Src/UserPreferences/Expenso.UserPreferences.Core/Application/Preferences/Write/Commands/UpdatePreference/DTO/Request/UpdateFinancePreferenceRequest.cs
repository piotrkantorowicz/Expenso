namespace Expenso.UserPreferences.Core.Application.Preferences.Write.Commands.UpdatePreference.DTO.Request;

public sealed record UpdateFinancePreferenceRequest(
    bool AllowAddFinancePlanSubOwners,
    int MaxNumberOfSubFinancePlanSubOwners,
    bool AllowAddFinancePlanReviewers,
    int MaxNumberOfFinancePlanReviewers);
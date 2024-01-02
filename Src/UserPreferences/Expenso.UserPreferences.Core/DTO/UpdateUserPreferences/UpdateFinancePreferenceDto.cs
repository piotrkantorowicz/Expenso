namespace Expenso.UserPreferences.Core.DTO.UpdateUserPreferences;

public sealed record UpdateFinancePreferenceDto(
    bool AllowAddFinancePlanSubOwners,
    int MaxNumberOfSubFinancePlanSubOwners,
    bool AllowAddFinancePlanReviewers,
    int MaxNumberOfFinancePlanReviewers);
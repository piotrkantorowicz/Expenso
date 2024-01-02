namespace Expenso.UserPreferences.Core.DTO.GetUserPreferences;

public sealed record FinancePreferenceDto(
    bool AllowAddFinancePlanSubOwners,
    int MaxNumberOfSubFinancePlanSubOwners,
    bool AllowAddFinancePlanReviewers,
    int MaxNumberOfFinancePlanReviewers);
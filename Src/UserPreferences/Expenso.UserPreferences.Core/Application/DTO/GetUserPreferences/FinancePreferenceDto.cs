namespace Expenso.UserPreferences.Core.Application.DTO.GetUserPreferences;

public sealed record FinancePreferenceDto(
    bool AllowAddFinancePlanSubOwners,
    int MaxNumberOfSubFinancePlanSubOwners,
    bool AllowAddFinancePlanReviewers,
    int MaxNumberOfFinancePlanReviewers);
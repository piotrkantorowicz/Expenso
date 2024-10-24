namespace Expenso.UserPreferences.Shared.DTO.MessageBus.UpdatePreference.FinancePreferences.Payload;

public sealed record FinancePreferenceUpdatedPayload(
    Guid UserId,
    bool AllowAddFinancePlanSubOwners,
    int MaxNumberOfSubFinancePlanSubOwners,
    bool AllowAddFinancePlanReviewers,
    int MaxNumberOfFinancePlanReviewers);
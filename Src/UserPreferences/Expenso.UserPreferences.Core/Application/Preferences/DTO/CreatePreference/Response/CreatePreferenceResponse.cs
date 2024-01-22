namespace Expenso.UserPreferences.Core.Application.Preferences.DTO.CreatePreference.Response;

public sealed record CreatePreferenceResponse(
    Guid Id,
    Guid UserId,
    CreateFinancePreferenceResponse FinancePreference,
    CreateNotificationPreferenceResponse NotificationPreference,
    CreateGeneralPreferenceResponse GeneralPreference);
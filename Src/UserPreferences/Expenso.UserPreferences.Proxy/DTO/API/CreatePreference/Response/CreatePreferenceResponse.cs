namespace Expenso.UserPreferences.Proxy.DTO.API.CreatePreference.Response;

public sealed record CreatePreferenceResponse(
    Guid Id,
    Guid UserId,
    CreateFinancePreferenceResponse FinancePreference,
    CreateNotificationPreferenceResponse NotificationPreference,
    CreateGeneralPreferenceResponse GeneralPreference);
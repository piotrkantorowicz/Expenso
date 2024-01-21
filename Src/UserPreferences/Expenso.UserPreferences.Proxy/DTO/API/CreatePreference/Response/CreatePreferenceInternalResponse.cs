namespace Expenso.UserPreferences.Proxy.DTO.API.CreatePreference.Response;

public sealed record CreatePreferenceInternalResponse(
    Guid Id,
    Guid UserId,
    CreateFinancePreferenceInternalResponse FinancePreference,
    CreateNotificationPreferenceInternalResponse NotificationPreference,
    CreateGeneralPreferenceInternalResponse GeneralPreference);
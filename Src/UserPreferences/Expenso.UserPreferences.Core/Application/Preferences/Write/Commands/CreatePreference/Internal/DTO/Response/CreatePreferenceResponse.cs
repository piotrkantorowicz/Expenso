namespace Expenso.UserPreferences.Core.Application.Preferences.Write.Commands.CreatePreference.Internal.DTO.Response;

public sealed record CreatePreferenceResponse(
    Guid Id,
    Guid UserId,
    CreateFinancePreferenceResponse FinancePreference,
    CreateNotificationPreferenceResponse NotificationPreference,
    CreateGeneralPreferenceResponse GeneralPreference);
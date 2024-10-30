namespace Expenso.UserPreferences.Shared.DTO.MessageBus.UpdatePreference.GeneralPreferences.Payload;

public sealed record GeneralPreferenceUpdatedPayload(Guid UserId, bool UseDarkMode);
using Expenso.UserPreferences.Core.Application.Preferences.DTO.CreatePreference.Response;
using Expenso.UserPreferences.Core.Application.Preferences.DTO.GetPreferences.Response;
using Expenso.UserPreferences.Core.Application.Preferences.DTO.UpdatePreferences.Request;
using Expenso.UserPreferences.Core.Domain.Preferences.Model.ValueObjects;
using Expenso.UserPreferences.Proxy.DTO.API.CreatePreference.Response;
using Expenso.UserPreferences.Proxy.DTO.API.GetPreference.Request;
using Expenso.UserPreferences.Proxy.DTO.MessageBus.NotificationPreferences;

namespace Expenso.UserPreferences.Core.Application.Preferences.Mappings;

internal static class NotificationPreferenceMap
{
    public static GetNotificationPreferenceResponse MapToGetResponse(NotificationPreference notificationPreference)
    {
        return new GetNotificationPreferenceResponse(notificationPreference.SendFinanceReportEnabled,
            notificationPreference.SendFinanceReportInterval);
    }

    public static CreateNotificationPreferenceResponse MapToCreateResponse(
        NotificationPreference notificationPreference)
    {
        return new CreateNotificationPreferenceResponse(notificationPreference.SendFinanceReportEnabled,
            notificationPreference.SendFinanceReportInterval);
    }

    public static NotificationPreference MapToModel(
        UpdateNotificationPreferenceRequest updateNotificationPreferenceRequest)
    {
        return NotificationPreference.Create(updateNotificationPreferenceRequest.SendFinanceReportEnabled,
            updateNotificationPreferenceRequest.SendFinanceReportInterval);
    }

    public static GetNotificationPreferenceInternalResponse MapToInternalGetRequest(
        NotificationPreference notificationPreference)
    {
        return new GetNotificationPreferenceInternalResponse(notificationPreference.SendFinanceReportEnabled,
            notificationPreference.SendFinanceReportInterval);
    }

    public static CreateNotificationPreferenceInternalResponse MapToInternalCreateResponse(
        NotificationPreference notificationPreference)
    {
        return new CreateNotificationPreferenceInternalResponse(notificationPreference.SendFinanceReportEnabled,
            notificationPreference.SendFinanceReportInterval);
    }

    public static NotificationPreferenceInternalContract MapToInternalContract(
        NotificationPreference notificationPreference)
    {
        return new NotificationPreferenceInternalContract(notificationPreference.SendFinanceReportEnabled,
            notificationPreference.SendFinanceReportInterval);
    }
}
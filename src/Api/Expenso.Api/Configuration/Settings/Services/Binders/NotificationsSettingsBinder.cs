using Expenso.Communication.Proxy.DTO.Settings;
using Expenso.Shared.System.Configuration.Binders;
using Expenso.Shared.System.Configuration.Sections;
using Expenso.Shared.System.Configuration.Services;
using Expenso.Shared.System.Configuration.Settings;

namespace Expenso.Api.Configuration.Settings.Services.Binders;

internal sealed class NotificationsSettingsBinder : ISettingsBinder
{
    private const string SectionName = SectionNames.Notifications;
    private readonly ISettingsService<NotificationSettings> _settingsService;

    public NotificationsSettingsBinder(ISettingsService<NotificationSettings> settingsService)
    {
        _settingsService = settingsService ?? throw new ArgumentNullException(paramName: nameof(settingsService));
    }

    public string GetSectionName()
    {
        return SectionName;
    }

    public ISettings Bind(IServiceCollection serviceCollection)
    {
        NotificationSettings settings = _settingsService.Bind(sectionName: SectionName);
        _settingsService.Validate();
        _settingsService.Register(serviceCollection: serviceCollection);

        return settings;
    }
}
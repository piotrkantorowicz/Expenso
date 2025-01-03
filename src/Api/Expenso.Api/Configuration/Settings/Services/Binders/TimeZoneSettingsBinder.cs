using Expenso.Api.Configuration.Settings.ApiSettings.TimeZone;
using Expenso.Shared.System.Configuration.Binders;
using Expenso.Shared.System.Configuration.Constants;
using Expenso.Shared.System.Configuration.Services;

namespace Expenso.Api.Configuration.Settings.Services.Binders;

internal sealed class TimeZoneSettingsBinder : ISettingsBinder
{
    private const string SectionName = SectionNames.TimeZones;
    private readonly ISettingsService<TimeZoneSettings> _settingsService;

    public TimeZoneSettingsBinder(ISettingsService<TimeZoneSettings> settingsService)
    {
        _settingsService = settingsService ?? throw new ArgumentNullException(paramName: nameof(settingsService));
    }

    public string GetSectionName()
    {
        return SectionName;
    }

    public object? Bind(IServiceCollection serviceCollection)
    {
        TimeZoneSettings? settings = _settingsService.Bind(sectionName: SectionName);
        _settingsService.Validate();
        _settingsService.Register(serviceCollection: serviceCollection);

        return settings;
    }
}
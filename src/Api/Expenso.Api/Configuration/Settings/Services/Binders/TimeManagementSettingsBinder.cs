using Expenso.Shared.System.Configuration.Binders;
using Expenso.Shared.System.Configuration.Constants;
using Expenso.Shared.System.Configuration.Services;
using Expenso.TimeManagement.Core.Application.Shared.Settings;

namespace Expenso.Api.Configuration.Settings.Services.Binders;

internal sealed class TimeManagementSettingsBinder : ISettingsBinder
{
    private const string SectionName = SectionNames.TimeManagement;
    private readonly ISettingsService<TimeManagementSettings> _settingsService;

    public TimeManagementSettingsBinder(ISettingsService<TimeManagementSettings> settingsService)
    {
        _settingsService = settingsService ?? throw new ArgumentNullException(paramName: nameof(settingsService));
    }

    public string GetSectionName()
    {
        return SectionName;
    }

    public object? Bind(IServiceCollection serviceCollection)
    {
        TimeManagementSettings? settings = _settingsService.Bind(sectionName: SectionName);
        _settingsService.Validate();
        _settingsService.Register(serviceCollection: serviceCollection);

        return settings;
    }
}
using Expenso.Shared.System.Configuration.Binders;
using Expenso.Shared.System.Configuration.Sections;
using Expenso.Shared.System.Configuration.Services;

namespace Expenso.Api.Configuration.Settings.Services.Binders;

internal sealed class CorsSettingsBinder : ISettingsBinder
{
    private const string SectionName = SectionNames.Cors;
    private readonly ISettingsService<CorsSettings> _settingsService;

    public CorsSettingsBinder(ISettingsService<CorsSettings> settingsService)
    {
        _settingsService = settingsService ?? throw new ArgumentNullException(paramName: nameof(settingsService));
    }

    public string GetSectionName()
    {
        return SectionName;
    }

    public object? Bind(IServiceCollection serviceCollection)
    {
        CorsSettings? settings = _settingsService.Bind(sectionName: SectionName);
        _settingsService.Validate();
        _settingsService.Register(serviceCollection: serviceCollection);

        return settings;
    }
}
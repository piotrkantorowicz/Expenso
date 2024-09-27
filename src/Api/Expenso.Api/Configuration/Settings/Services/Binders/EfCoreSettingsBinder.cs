using Expenso.Shared.Database.EfCore.Settings;
using Expenso.Shared.System.Configuration.Binders;
using Expenso.Shared.System.Configuration.Sections;
using Expenso.Shared.System.Configuration.Services;

namespace Expenso.Api.Configuration.Settings.Services.Binders;

internal sealed class EfCoreSettingsBinder : ISettingsBinder
{
    private const string SectionName = SectionNames.EfCore;
    private readonly ISettingsService<EfCoreSettings> _settingsService;

    public EfCoreSettingsBinder(ISettingsService<EfCoreSettings> settingsService)
    {
        _settingsService = settingsService ?? throw new ArgumentNullException(paramName: nameof(settingsService));
    }

    public string GetSectionName()
    {
        return SectionName;
    }

    public object? Bind(IServiceCollection serviceCollection)
    {
        EfCoreSettings? settings = _settingsService.Bind(sectionName: SectionName);
        _settingsService.Validate();
        _settingsService.Register(serviceCollection: serviceCollection);

        return settings;
    }
}
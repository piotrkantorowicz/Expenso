using Expenso.Shared.System.Configuration.Binders;
using Expenso.Shared.System.Configuration.Sections;
using Expenso.Shared.System.Configuration.Services;
using Expenso.Shared.System.Configuration.Settings.App;

namespace Expenso.Api.Configuration.Settings.Services.Binders;

internal sealed class ApplicationSettingsBinder : ISettingsBinder
{
    private const string SectionName = SectionNames.Application;
    private readonly ISettingsService<ApplicationSettings> _settingsService;

    public ApplicationSettingsBinder(ISettingsService<ApplicationSettings> settingsService)
    {
        _settingsService = settingsService ?? throw new ArgumentNullException(paramName: nameof(settingsService));
    }

    public string GetSectionName()
    {
        return SectionName;
    }

    public object? Bind(IServiceCollection serviceCollection)
    {
        ApplicationSettings? settings = _settingsService.Bind(sectionName: SectionName);
        _settingsService.Validate();
        _settingsService.Register(serviceCollection: serviceCollection);

        return settings;
    }
}
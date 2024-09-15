using Expenso.Shared.System.Configuration.Binders;
using Expenso.Shared.System.Configuration.Sections;
using Expenso.Shared.System.Configuration.Services;
using Expenso.Shared.System.Configuration.Settings;
using Expenso.Shared.System.Configuration.Settings.Files;

namespace Expenso.Api.Configuration.Settings.Services.Binders;

internal sealed class FileSettingsBinder : ISettingsBinder
{
    private const string SectionName = SectionNames.Files;
    private readonly ISettingsService<FilesSettings> _settingsService;

    public FileSettingsBinder(ISettingsService<FilesSettings> settingsService)
    {
        _settingsService = settingsService ?? throw new ArgumentNullException(paramName: nameof(settingsService));
    }

    public string GetSectionName()
    {
        return SectionName;
    }

    public ISettings Bind(IServiceCollection serviceCollection)
    {
        FilesSettings settings = _settingsService.Bind(sectionName: SectionName);
        _settingsService.Validate();
        _settingsService.Register(serviceCollection: serviceCollection);

        return settings;
    }
}
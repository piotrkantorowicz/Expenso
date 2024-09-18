using Expenso.Shared.System.Configuration.Binders;
using Expenso.Shared.System.Configuration.Sections;
using Expenso.Shared.System.Configuration.Services;
using Expenso.Shared.System.Configuration.Settings.Auth;

namespace Expenso.Api.Configuration.Settings.Services.Binders;

internal sealed class AuthSettingsBinder : ISettingsBinder
{
    private const string SectionName = SectionNames.Auth;
    private readonly ISettingsService<AuthSettings> _settingsService;

    public AuthSettingsBinder(ISettingsService<AuthSettings> settingsService)
    {
        _settingsService = settingsService ?? throw new ArgumentNullException(paramName: nameof(settingsService));
    }

    public string GetSectionName()
    {
        return SectionName;
    }

    public object? Bind(IServiceCollection serviceCollection)
    {
        AuthSettings? settings = _settingsService.Bind(sectionName: SectionName);
        _settingsService.Validate();
        _settingsService.Register(serviceCollection: serviceCollection);

        return settings;
    }
}
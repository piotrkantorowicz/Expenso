using Expenso.IAM.Core.Acl.Keycloak;
using Expenso.Shared.System.Configuration.Binders;
using Expenso.Shared.System.Configuration.Sections;
using Expenso.Shared.System.Configuration.Services;

namespace Expenso.Api.Configuration.Settings.Services.Binders;

internal sealed class KeycloakSettingsBinder : ISettingsBinder
{
    private const string SectionName = SectionNames.Keycloak;
    private readonly ISettingsService<KeycloakSettings> _settingsService;

    public KeycloakSettingsBinder(ISettingsService<KeycloakSettings> settingsService)
    {
        _settingsService = settingsService ?? throw new ArgumentNullException(paramName: nameof(settingsService));
    }

    public string GetSectionName()
    {
        return SectionName;
    }

    public object? Bind(IServiceCollection serviceCollection)
    {
        KeycloakSettings? settings = _settingsService.Bind(sectionName: SectionName);
        _settingsService.Validate();
        _settingsService.Register(serviceCollection: serviceCollection);

        return settings;
    }
}
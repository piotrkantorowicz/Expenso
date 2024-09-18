using Expenso.Shared.System.Configuration.Binders;
using Expenso.Shared.System.Configuration.Sections;
using Expenso.Shared.System.Configuration.Services;
using Expenso.Shared.System.Metrics;

namespace Expenso.Api.Configuration.Settings.Services.Binders;

internal sealed class OtlpSettingsBinder : ISettingsBinder
{
    private const string SectionName = SectionNames.Otlp;
    private readonly ISettingsService<OtlpSettings> _settingsService;

    public OtlpSettingsBinder(ISettingsService<OtlpSettings> settingsService)
    {
        _settingsService = settingsService ?? throw new ArgumentNullException(paramName: nameof(settingsService));
    }

    public string GetSectionName()
    {
        return SectionName;
    }

    public object? Bind(IServiceCollection serviceCollection)
    {
        OtlpSettings? settings = _settingsService.Bind(sectionName: SectionName);
        _settingsService.Validate();
        _settingsService.Register(serviceCollection: serviceCollection);

        return settings;
    }
}
using Expenso.BudgetSharing.Application.Shared.Settings;
using Expenso.Shared.System.Configuration.Binders;
using Expenso.Shared.System.Configuration.Sections;
using Expenso.Shared.System.Configuration.Services;

namespace Expenso.Api.Configuration.Settings.Services.Binders;

internal sealed class BudgetSharingSettingsBinder : ISettingsBinder
{
    private const string SectionName = SectionNames.BudgetSharing;
    private readonly ISettingsService<BudgetSharingSettings> _settingsService;

    public BudgetSharingSettingsBinder(ISettingsService<BudgetSharingSettings> settingsService)
    {
        _settingsService = settingsService ?? throw new ArgumentNullException(paramName: nameof(settingsService));
    }

    public string GetSectionName()
    {
        return SectionName;
    }

    public object? Bind(IServiceCollection serviceCollection)
    {
        BudgetSharingSettings? settings = _settingsService.Bind(sectionName: SectionName);
        _settingsService.Validate();
        _settingsService.Register(serviceCollection: serviceCollection);

        return settings;
    }
}
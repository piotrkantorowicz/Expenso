using Expenso.Shared.System.Configuration.Settings;

using Microsoft.Extensions.DependencyInjection;

namespace Expenso.Shared.System.Configuration.Binders;

public interface ISettingsBinder
{
    string GetSectionName();

    ISettings Bind(IServiceCollection serviceCollection);
}
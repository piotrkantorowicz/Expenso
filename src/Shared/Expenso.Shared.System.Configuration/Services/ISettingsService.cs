using Microsoft.Extensions.DependencyInjection;

namespace Expenso.Shared.System.Configuration.Services;

public interface ISettingsService<out TSettings>
{
    void Validate();

    TSettings? Bind(string sectionName);

    void Register(IServiceCollection serviceCollection);
}
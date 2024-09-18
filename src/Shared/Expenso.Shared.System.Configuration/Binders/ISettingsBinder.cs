using Microsoft.Extensions.DependencyInjection;

namespace Expenso.Shared.System.Configuration.Binders;

public interface ISettingsBinder
{
    string GetSectionName();

    object? Bind(IServiceCollection serviceCollection);
}
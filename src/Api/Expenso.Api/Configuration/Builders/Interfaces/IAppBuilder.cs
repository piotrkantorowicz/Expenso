using Expenso.Api.Configuration.Configurators.Interfaces;

namespace Expenso.Api.Configuration.Builders.Interfaces;

internal interface IAppBuilder
{
    IAppBuilder ConfigureApiDependencies();

    IAppBuilder ConfigureModules();

    IAppBuilder ConfigureSharedFramework();

    IAppBuilder ConfigureHealthChecks();

    IAppBuilder ConfigureMvc();

    IAppBuilder ConfigureCors();

    IAppBuilder ConfigureCache();

    IAppBuilder ConfigureSerializationOptions();

    IAppBuilder ConfigureAuthorization();

    IAppBuilder ConfigureSwagger();

    IAppConfigurator Build();
}
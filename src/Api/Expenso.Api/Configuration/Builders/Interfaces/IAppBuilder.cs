namespace Expenso.Api.Configuration.Builders.Interfaces;

internal interface IAppBuilder
{
    IAppBuilder ConfigureApiDependencies();

    IAppBuilder ConfigureModules();

    IAppBuilder ConfigureSharedFramework();

    IAppBuilder ConfigureMvc();

    IAppBuilder ConfigureSerializationOptions();

    IAppBuilder ConfigureAuthorization();

    IAppBuilder ConfigureSwagger();

    IAppConfigurator Build();
}
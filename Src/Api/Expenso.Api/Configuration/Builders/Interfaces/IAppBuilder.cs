namespace Expenso.Api.Configuration.Builders.Interfaces;

internal interface IAppBuilder
{
    IAppBuilder ConfigureModules();

    IAppBuilder ConfigureApiDependencies();

    IAppBuilder ConfigureMvc();

    IAppBuilder ConfigureSerializationOptions();

    IAppBuilder ConfigureKeycloak();

    IAppBuilder ConfigureSwagger();

    IAppBuilder ConfigureMessageBroker();

    IAppConfigurator Build();
}
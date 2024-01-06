namespace Expenso.Api.Configuration.Builders.Interfaces;

internal interface IAppBuilder
{
    IAppBuilder ConfigureApiDependencies();

    IAppBuilder ConfigureModules();

    IAppBuilder ConfigureMvc();

    IAppBuilder ConfigureSerializationOptions();

    IAppBuilder ConfigureKeycloak();

    IAppBuilder ConfigureSwagger();

    IAppBuilder ConfigureMessageBroker();

    IAppConfigurator Build();
}
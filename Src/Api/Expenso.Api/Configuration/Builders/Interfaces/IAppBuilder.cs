namespace Expenso.Api.Configuration.Builders.Interfaces;

internal interface IAppBuilder
{
    IAppBuilder ConfigureApiDependencies();

    IAppBuilder ConfigureModules();

    IAppBuilder ConfigureMvc();

    IAppBuilder ConfigureSerializationOptions();

    IAppBuilder ConfigureAuthorization();

    IAppBuilder ConfigureSwagger();

    IAppBuilder ConfigureMessageBroker();

    IAppConfigurator Build();
}
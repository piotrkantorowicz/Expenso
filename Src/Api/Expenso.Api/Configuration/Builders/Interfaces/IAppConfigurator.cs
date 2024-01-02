namespace Expenso.Api.Configuration.Builders.Interfaces;

internal interface IAppConfigurator
{
    IAppConfigurator UseSwagger();

    IAppConfigurator UseAuth();

    IAppConfigurator UseHttpsRedirection();

    IAppConfigurator CreateEndpoints();

    IAppConfigurator MigrateDatabase();

    void Run();
}
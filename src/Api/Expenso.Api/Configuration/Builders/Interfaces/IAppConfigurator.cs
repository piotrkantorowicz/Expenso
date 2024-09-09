namespace Expenso.Api.Configuration.Builders.Interfaces;

internal interface IAppConfigurator
{
    IAppConfigurator UseSwagger();

    IAppConfigurator UseAuth();

    IAppConfigurator UseHealthChecks();

    IAppConfigurator UseHttpsRedirection();

    IAppConfigurator UseRequestsCorrelation();

    IAppConfigurator UseErrorHandler();

    IAppConfigurator UseResolvers();

    IAppConfigurator CreateEndpoints();

    IAppConfigurator MigrateDatabase();

    IAppConfigurator UseRequestsLogging();

    void Run();
}
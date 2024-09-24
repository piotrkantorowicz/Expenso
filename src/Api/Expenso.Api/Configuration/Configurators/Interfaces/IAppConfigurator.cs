namespace Expenso.Api.Configuration.Configurators.Interfaces;

internal interface IAppConfigurator
{
    IAppConfigurator UseSwagger();

    IAppConfigurator UseAuth();

    IAppConfigurator UseCors();

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
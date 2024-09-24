using Expenso.Api.Configuration.Builders;

AppBuilder appBuilder = new(appBuilder: WebApplication.CreateBuilder(args: args));

appBuilder
    .ConfigureModules()
    .ConfigureApiDependencies()
    .ConfigureCors()
    .ConfigureSharedFramework()
    .ConfigureMvc()
    .ConfigureHealthChecks()
    .ConfigureSerializationOptions()
    .ConfigureAuthorization()
    .ConfigureSwagger()
    .ConfigureCache()
    .Build()
    .CreateEndpoints()
    .UseCors()
    .UseAuth()
    .UseHealthChecks()
    .UseHttpsRedirection()
    .UseRequestsCorrelation()
    .UseRequestsLogging()
    .UseErrorHandler()
    .UseResolvers()
    .UseSwagger()
    .MigrateDatabase()
    .Run();
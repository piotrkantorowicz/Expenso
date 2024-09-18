using Expenso.Api.Configuration.Builders;

AppBuilder appBuilder = new(appBuilder: WebApplication.CreateBuilder(args: args));

appBuilder
    .ConfigureModules()
    .ConfigureApiDependencies()
    .ConfigureSharedFramework()
    .ConfigureMvc()
    .ConfigureHealthChecks()
    .ConfigureSerializationOptions()
    .ConfigureAuthorization()
    .ConfigureSwagger()
    .ConfigureCache()
    .Build()
    .CreateEndpoints()
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
using Expenso.Api.Configuration.Builders;

AppBuilder appBuilder = new(args: args);

appBuilder
    .ConfigureApiDependencies()
    .ConfigureModules()
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
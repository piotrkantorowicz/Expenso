using Expenso.Api.Configuration.Builders;

AppBuilder appBuilder = new(args: args);

appBuilder
    .ConfigureApiDependencies()
    .ConfigureModules()
    .ConfigureSharedFramework()
    .ConfigureMvc()
    .ConfigureSerializationOptions()
    .ConfigureAuthorization()
    .ConfigureSwagger()
    .Build()
    .CreateEndpoints()
    .UseAuth()
    .UseHttpsRedirection()
    .UseRequestsCorrelation()
    .UseErrorHandler()
    .UseResolvers()
    .UseSwagger()
    .MigrateDatabase()
    .Run();
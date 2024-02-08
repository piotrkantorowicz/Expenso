using Expenso.Api.Configuration.Builders;

AppBuilder appBuilder = new(args);

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
    .UseErrorHandler()
    .UseSwagger()
    .MigrateDatabase()
    .Run();
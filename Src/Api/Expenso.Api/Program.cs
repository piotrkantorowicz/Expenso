using Expenso.Api.Configuration.Builders;

AppBuilder appBuilder = new(args);

appBuilder
    .ConfigureApiDependencies()
    .ConfigureModules()
    .ConfigureMvc()
    .ConfigureSerializationOptions()
    .ConfigureAuthorization()
    .ConfigureCqrs()
    .ConfigureMessageBroker()
    .ConfigureSwagger()
    .Build()
    .CreateEndpoints()
    .UseAuth()
    .UseHttpsRedirection()
    .UseErrorHandler()
    .UseSwagger()
    .MigrateDatabase()
    .Run();
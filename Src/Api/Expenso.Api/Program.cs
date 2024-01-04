using Expenso.Api.Configuration.Builders;

AppBuilder appBuilder = new(args);

appBuilder
    .ConfigureApiDependencies()
    .ConfigureModules()
    .ConfigureMvc()
    .ConfigureSerializationOptions()
    .ConfigureKeycloak()
    .ConfigureMessageBroker()
    .ConfigureSwagger()
    .Build()
    .CreateEndpoints()
    .UseAuth()
    .UseHttpsRedirection()
    .UseSwagger()
    .MigrateDatabase()
    .Run();
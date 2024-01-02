using Expenso.Api.Configuration.Builders;

AppBuilder appBuilder = new(args);

appBuilder
    .ConfigureModules()
    .ConfigureApiDependencies()
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
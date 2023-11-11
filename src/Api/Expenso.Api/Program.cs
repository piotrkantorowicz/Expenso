using Expenso.Api.Configuration.Builders;

AppBuilder appBuilder = new AppBuilder(args);

appBuilder.ConfigureModules().ConfigureApiDependencies().ConfigureMvc().ConfigureSerializationOptions()
    .ConfigureKeycloak().ConfigureSwagger().Build().CreateEndpoints().UseAuth().UseHttpsRedirection().UseSwagger()
    .Run();
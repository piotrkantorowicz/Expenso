using Expenso.Api.Configuration;
using Expenso.Api.Configuration.Builders;
using Expenso.Api.Configuration.Settings.Services.Containers;
using Expenso.BudgetSharing.Api;
using Expenso.Communication.Api;
using Expenso.DocumentManagement.Api;
using Expenso.IAM.Api;
using Expenso.Shared.System.Modules;
using Expenso.TimeManagement.Api;
using Expenso.UserPreferences.Api;

WebApplicationBuilder webApplicationBuilder = WebApplication.CreateBuilder(args: args);

Modules.RegisterModules(moduleFactories:
[
    () => new IamModule(),
    () => new UserPreferencesModule(),
    () => new BudgetSharingModule(),
    () => new DocumentManagementModule(),
    () => new CommunicationModule(),
    () => new TimeManagementModule()
]);

PreStartupContainer preStartupContainer = new();

preStartupContainer.Build(configuration: webApplicationBuilder.Configuration,
    assemblies: Modules.GetRequiredModulesAssemblies(merge: [typeof(Program).Assembly]));

AppConfigurationManager appConfigurationManager = new(preStartupContainer: preStartupContainer);
appConfigurationManager.Configure(serviceCollection: webApplicationBuilder.Services);

new AppBuilder(appBuilder: webApplicationBuilder, configuration: webApplicationBuilder.Configuration,
        serviceCollection: webApplicationBuilder.Services, appConfigurationManager: appConfigurationManager)
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
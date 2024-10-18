using System.Reflection;

using Expenso.Shared.Commands;
using Expenso.Shared.Commands.Dispatchers;
using Expenso.Shared.Commands.Validation;
using Expenso.Shared.System.Configuration.Settings.App;
using Expenso.Shared.System.Logging;

using Microsoft.Extensions.DependencyInjection;

namespace Expenso.Shared.Tests.UnitTests.Commands.CommandDispatcher;

[TestFixture]
internal abstract class CommandDispatcherTestBase : TestBase<ICommandDispatcher>
{
    [SetUp]
    public void Setup()
    {
        Assembly[] assemblies = [typeof(CommandDispatcherTestBase).Assembly];

        IServiceCollection serviceCollection = new ServiceCollection()
            .AddCommands(assemblies: assemblies)
            .AddCommandsValidations(assemblies: assemblies)
            .AddLogging()
            .AddInternalLogging();

        serviceCollection.AddSingleton<ApplicationSettings>();
        IServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();
        TestCandidate = new Shared.Commands.Dispatchers.CommandDispatcher(serviceProvider: serviceProvider);
    }
}
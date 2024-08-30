using System.Reflection;

using Expenso.Shared.Commands;
using Expenso.Shared.Commands.Dispatchers;
using Expenso.Shared.Commands.Validation;
using Expenso.Shared.System.Configuration.Settings;
using Expenso.Shared.System.Logging;

using Microsoft.Extensions.DependencyInjection;

using TestCandidate = Expenso.Shared.Commands.Dispatchers.CommandDispatcher;

namespace Expenso.Shared.Tests.UnitTests.Commands.CommandDispatcher;

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
        TestCandidate = new TestCandidate(serviceProvider: serviceProvider);
    }
}
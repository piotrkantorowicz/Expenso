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
    private IServiceProvider? _serviceProvider;

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
        _serviceProvider = serviceCollection.BuildServiceProvider();
        TestCandidate = new Shared.Commands.Dispatchers.CommandDispatcher(serviceProvider: _serviceProvider);
    }

    [TearDown]
    public void TearDown()
    {
        if (_serviceProvider is IDisposable disposable)
        {
            disposable.Dispose();
        }

        _serviceProvider = null;
        TestCandidate = null!;
    }
}
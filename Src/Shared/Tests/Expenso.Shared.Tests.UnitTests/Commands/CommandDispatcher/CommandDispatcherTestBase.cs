using Expenso.Shared.Commands;
using Expenso.Shared.Commands.Dispatchers;
using Expenso.Shared.Commands.Validation;

using Microsoft.Extensions.DependencyInjection;

using TestCandidate = Expenso.Shared.Commands.Dispatchers.CommandDispatcher;

namespace Expenso.Shared.Tests.UnitTests.Commands.CommandDispatcher;

internal abstract class CommandDispatcherTestBase : TestBase<ICommandDispatcher>
{
    [SetUp]
    public void Setup()
    {
        ServiceProvider serviceProvider = new ServiceCollection()
            .AddCommands()
            .AddCommandsValidation()
            .AddLogging()
            .BuildServiceProvider();

        TestCandidate = new TestCandidate(serviceProvider);
    }
}
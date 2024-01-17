using Expenso.Shared.Commands;

using Microsoft.Extensions.DependencyInjection;

namespace Expenso.Shared.Tests.UnitTests.Commands.CommandDispatchers;

internal abstract class CommandDispatcherTestBase : TestBase<ICommandDispatcher>
{
    [SetUp]
    public void Setup()
    {
        ServiceProvider serviceProvider = new ServiceCollection().AddCommands().AddLogging().BuildServiceProvider();
        TestCandidate = new CommandDispatcher(serviceProvider);
    }
}
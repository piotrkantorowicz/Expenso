using Expenso.Shared.Commands;
using Expenso.Shared.Commands.Dispatchers;

using Microsoft.Extensions.DependencyInjection;

using TestCandidate = Expenso.Shared.Commands.Dispatchers.CommandDispatcher;

namespace Expenso.Shared.Tests.UnitTests.Commands.CommandDispatcher;

internal abstract class CommandDispatcherTestBase : TestBase<ICommandDispatcher>
{
    [SetUp]
    public void Setup()
    {
        ServiceProvider serviceProvider = new ServiceCollection().AddCommands().AddLogging().BuildServiceProvider();
        TestCandidate = new TestCandidate(serviceProvider);
    }
}
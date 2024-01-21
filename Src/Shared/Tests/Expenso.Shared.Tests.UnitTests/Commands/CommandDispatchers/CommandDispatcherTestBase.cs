using Expenso.Shared.Commands;
using Expenso.Shared.Commands.Dispatchers;

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

    internal sealed record TestCommand(Guid Id) : ICommand;

    internal sealed record TestCommandResult(string Message);
}
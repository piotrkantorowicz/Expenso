using System.Text;

using Expenso.Shared.Commands;

namespace Expenso.Shared.Tests.UnitTests.Commands.CommandHandlers.Result;

internal abstract class CommandHandlerResultTestBase : TestBase<TestCommandHandler>
{
    protected TestCommand _testCommand = null!;

    [SetUp]
    protected void Setup()
    {
        _testCommand = new TestCommand(Guid.NewGuid());
        TestCandidate = new TestCommandHandler();
    }
}

internal sealed record TestCommand(Guid Id) : ICommand;

internal sealed record CommandResult(string Message);

internal sealed class TestCommandHandler : ICommandHandler<TestCommand, CommandResult>
{
    public async Task<CommandResult?> HandleAsync(TestCommand command, CancellationToken cancellationToken = default)
    {
        string message = new StringBuilder()
            .Append("Successfully processed command with id: ")
            .Append(command.Id)
            .ToString();

        return await Task.FromResult(new CommandResult(message));
    }
}
using Expenso.Shared.Commands;

namespace Expenso.Shared.Tests.UnitTests.Commands.TestData.Result;

internal sealed class TestCommandHandler : ICommandHandler<TestCommand, TestCommandResult>
{
    public async Task<TestCommandResult> HandleAsync(TestCommand command, CancellationToken cancellationToken)
    {
        string message = $"Successfully processed command with ID {command.Id}";

        return await Task.FromResult(result: new TestCommandResult(Message: message));
    }
}
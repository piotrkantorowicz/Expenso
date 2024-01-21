using System.Text;

using Expenso.Shared.Commands;

namespace Expenso.Shared.Tests.UnitTests.Commands.TestData.Result;

internal sealed class TestCommandHandler : ICommandHandler<TestCommand, TestCommandResult>
{
    public async Task<TestCommandResult?> HandleAsync(TestCommand command,
        CancellationToken cancellationToken = default)
    {
        string message = new StringBuilder()
            .Append("Successfully processed command with id: ")
            .Append(command.Id)
            .ToString();

        return await Task.FromResult(new TestCommandResult(message));
    }
}
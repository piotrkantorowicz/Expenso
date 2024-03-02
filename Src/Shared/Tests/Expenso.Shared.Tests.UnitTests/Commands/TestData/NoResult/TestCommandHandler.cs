using Expenso.Shared.Commands;

using Microsoft.Extensions.Logging;

namespace Expenso.Shared.Tests.UnitTests.Commands.TestData.NoResult;

internal sealed class TestCommandHandler(ILogger<TestCommandHandler> logger) : ICommandHandler<TestCommand>
{
    private readonly ILogger<TestCommandHandler> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    public async Task HandleAsync(TestCommand command, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Successfully processed command with id: {CommandId}", command.Id);
        await Task.CompletedTask;
    }
}
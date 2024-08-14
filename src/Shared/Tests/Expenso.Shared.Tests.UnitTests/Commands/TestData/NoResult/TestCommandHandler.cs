using Expenso.Shared.Commands;

using Microsoft.Extensions.Logging;

namespace Expenso.Shared.Tests.UnitTests.Commands.TestData.NoResult;

internal sealed class TestCommandHandler : ICommandHandler<TestCommand>
{
    private readonly ILogger<TestCommandHandler> _logger;

    public TestCommandHandler(ILogger<TestCommandHandler> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(paramName: nameof(logger));
    }

    public async Task HandleAsync(TestCommand command, CancellationToken cancellationToken)
    {
        _logger.LogInformation(message: "Successfully processed command with id: {CommandId}", command.Id);
        await Task.CompletedTask;
    }
}
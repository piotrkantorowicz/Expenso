using Expenso.Shared.Commands;

using Microsoft.Extensions.Logging;

using Moq;

namespace Expenso.Shared.Tests.UnitTests.Commands.CommandHandlers.NoResult;

internal abstract class CommandHandlerNoResultTestBase : TestBase<TestCommandHandler>
{
    protected Mock<ILogger<TestCommandHandler>> _loggerMock = null!;
    protected TestCommand _testCommand = null!;

    [SetUp]
    protected void Setup()
    {
        _testCommand = new TestCommand(Guid.NewGuid());
        _loggerMock = new Mock<ILogger<TestCommandHandler>>();
        TestCandidate = new TestCommandHandler(_loggerMock.Object);
    }
}

internal sealed record TestCommand(Guid Id) : ICommand;

internal sealed class TestCommandHandler(ILogger<TestCommandHandler> logger) : ICommandHandler<TestCommand>
{
    private readonly ILogger<TestCommandHandler> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    public async Task HandleAsync(TestCommand command, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Successfully processed command with id: {CommandId}", command.Id);
        await Task.CompletedTask;
    }
}
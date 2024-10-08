using Expenso.Shared.Commands;
using Expenso.Shared.System.Logging;

namespace Expenso.Shared.Tests.UnitTests.Commands.TestData.NoResult;

internal sealed class TestCommandHandler : ICommandHandler<TestCommand>
{
    private readonly ILoggerService<TestCommandHandler> _logger;

    public TestCommandHandler(ILoggerService<TestCommandHandler> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(paramName: nameof(logger));
    }

    public async Task HandleAsync(TestCommand command, CancellationToken cancellationToken)
    {
        _logger.LogInfo(eventId: LoggingUtils.GeneralInformation,
            message: "Successfully processed command with ID {CommandId}", messageContext: command.MessageContext,
            args: command.Id);

        await Task.CompletedTask;
    }
}
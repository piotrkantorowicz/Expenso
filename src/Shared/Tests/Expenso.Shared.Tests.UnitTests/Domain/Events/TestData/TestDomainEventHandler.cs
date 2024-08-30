using Expenso.Shared.Domain.Events;
using Expenso.Shared.System.Logging;

namespace Expenso.Shared.Tests.UnitTests.Domain.Events.TestData;

internal sealed class TestDomainEventHandler : IDomainEventHandler<TestDomainEvent>
{
    private readonly ILoggerService<TestDomainEventHandler> _logger;

    public TestDomainEventHandler(ILoggerService<TestDomainEventHandler> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(paramName: nameof(logger));
    }

    public async Task HandleAsync(TestDomainEvent @event, CancellationToken cancellationToken)
    {
        _logger.LogInfo(eventId: LoggingUtils.GeneralInformation,
            message: "Successfully handled @event with id: {EventId} and name: {EventName}",
            messageContext: @event.MessageContext, args: [@event.Id, @event.Name]);

        await Task.CompletedTask;
    }
}
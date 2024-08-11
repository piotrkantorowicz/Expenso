using Expenso.Shared.Domain.Events;

using Microsoft.Extensions.Logging;

namespace Expenso.Shared.Tests.UnitTests.Domain.Events.TestData;

internal sealed class TestDomainEventHandler(ILogger<TestDomainEventHandler> logger)
    : IDomainEventHandler<TestDomainEvent>
{
    private readonly ILogger<TestDomainEventHandler> _logger =
        logger ?? throw new ArgumentNullException(paramName: nameof(logger));

    public async Task HandleAsync(TestDomainEvent @event, CancellationToken cancellationToken)
    {
        _logger.LogInformation(message: "Successfully handled @event with id: {EventId} and name: {EventName}",
            @event.Id, @event.Name);

        await Task.CompletedTask;
    }
}
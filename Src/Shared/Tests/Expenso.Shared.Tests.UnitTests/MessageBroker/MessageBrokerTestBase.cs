using Expenso.Shared.IntegrationEvents;
using Expenso.Shared.MessageBroker;
using Expenso.Shared.MessageBroker.InMemory;
using Expenso.Shared.MessageBroker.InMemory.Channels;

namespace Expenso.Shared.Tests.UnitTests.MessageBroker;

internal abstract class MessageBrokerTestBase : TestBase
{
    private readonly IMessageChannel _messageChannel = new MessageChannel();
    private readonly CancellationTokenSource _stoppingTokenSource = new();
    private Task? _subscriberTask;

    protected IMessageBroker TestCandidate { get; private set; } = null!;

    [SetUp]
    public async Task SetUp()
    {
        TestCandidate = new InMemoryMessageBroker(_messageChannel);
        await StartSubscriber(AssertSubscribedEvent, _stoppingTokenSource.Token);
    }

    [TearDown]
    public async Task TearDown()
    {
        _messageChannel.Writer.Complete();
        await _stoppingTokenSource.CancelAsync();

        // Simulate a delay to allow the subscriber to complete
        await Task.Delay(1000);
        _subscriberTask?.Dispose();
    }

    protected abstract Task AssertSubscribedEvent<TIntegrationEvent>(TIntegrationEvent integrationEvent,
        CancellationToken cancellationToken);

    private async Task StartSubscriber(Func<IIntegrationEvent, CancellationToken, Task> handler,
        CancellationToken cancellationToken)
    {
        _subscriberTask = await Task.Factory.StartNew(() => SubscribeAsync(handler, cancellationToken),
            cancellationToken);
    }

    private async Task SubscribeAsync<TIntegrationEvent>(Func<TIntegrationEvent, CancellationToken, Task> handler,
        CancellationToken cancellationToken) where TIntegrationEvent : IIntegrationEvent
    {
        var reader = _messageChannel.Reader;

        while (await reader.WaitToReadAsync(cancellationToken))
        {
            while (reader.TryRead(out var message))
            {
                if (message is TIntegrationEvent integrationEvent)
                {
                    await handler(integrationEvent, cancellationToken);
                }
            }
        }
    }
}
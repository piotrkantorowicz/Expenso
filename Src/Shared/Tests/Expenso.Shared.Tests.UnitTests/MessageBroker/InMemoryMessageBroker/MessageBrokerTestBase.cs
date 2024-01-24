using Expenso.Shared.IntegrationEvents;
using Expenso.Shared.MessageBroker;
using Expenso.Shared.MessageBroker.InMemory.Background;
using Expenso.Shared.MessageBroker.InMemory.Channels;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

using TestCandidate = Expenso.Shared.MessageBroker.InMemory.InMemoryMessageBroker;

namespace Expenso.Shared.Tests.UnitTests.MessageBroker.InMemoryMessageBroker;

internal abstract class MessageBrokerTestBase : TestBase<IMessageBroker>
{
    private readonly IMessageChannel _messageChannel = new MessageChannel();
    private readonly CancellationTokenSource _stoppingTokenSource = new();
    private BackgroundMessageProcessor? _backgroundMessageProcessor;

    [SetUp]
    public async Task SetUp()
    {
        TestCandidate = new TestCandidate(_messageChannel);
        await StartMessageProcessor(_stoppingTokenSource.Token);
    }

    [TearDown]
    public async Task TearDown()
    {
        _messageChannel.Writer.Complete();
        await _backgroundMessageProcessor?.StopAsync(_stoppingTokenSource.Token)!;
        _backgroundMessageProcessor?.Dispose();
    }

    private async Task StartMessageProcessor(CancellationToken cancellationToken)
    {
        ServiceProvider serviceProvider = new ServiceCollection()
            .Scan(s => s
                .FromAssemblies(AppDomain.CurrentDomain.GetAssemblies())
                .AddClasses(c => c.AssignableTo(typeof(IIntegrationEventHandler<>)))
                .AsImplementedInterfaces()
                .WithScopedLifetime())
            .BuildServiceProvider();

        NullLoggerFactory loggerFactory = new();

        _backgroundMessageProcessor = new BackgroundMessageProcessor(_messageChannel, serviceProvider,
            loggerFactory.CreateLogger<BackgroundMessageProcessor>());

        await _backgroundMessageProcessor.StartAsync(cancellationToken);
    }
}

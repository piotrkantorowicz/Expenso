using Expenso.Shared.Integration.Events;
using Expenso.Shared.Integration.MessageBroker;
using Expenso.Shared.Integration.MessageBroker.InMemory.Background;
using Expenso.Shared.Integration.MessageBroker.InMemory.Channels;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

using TestCandidate = Expenso.Shared.Integration.MessageBroker.InMemory.InMemoryMessageBroker;

namespace Expenso.Shared.Tests.UnitTests.Integration.MessageBroker.InMemoryMessageBroker;

internal abstract class MessageBrokerTestBase : TestBase<IMessageBroker>
{
    private readonly IMessageChannel _messageChannel = new MessageChannel();
    private readonly CancellationTokenSource _stoppingTokenSource = new();
    private BackgroundMessageProcessor? _backgroundMessageProcessor;

    [SetUp]
    public async Task SetUp()
    {
        TestCandidate = new TestCandidate(channel: _messageChannel);
        await StartMessageProcessor(cancellationToken: _stoppingTokenSource.Token);
    }

    [TearDown]
    public async Task TearDown()
    {
        _messageChannel.Writer.Complete();
        await _backgroundMessageProcessor?.StopAsync(cancellationToken: _stoppingTokenSource.Token)!;
        _backgroundMessageProcessor?.Dispose();
    }

    private async Task StartMessageProcessor(CancellationToken cancellationToken)
    {
        ServiceProvider serviceProvider = new ServiceCollection()
            .Scan(action: s => s
                .FromAssemblies(assemblies: AppDomain.CurrentDomain.GetAssemblies())
                .AddClasses(action: c => c.AssignableTo(type: typeof(IIntegrationEventHandler<>)))
                .AsImplementedInterfaces()
                .WithScopedLifetime())
            .BuildServiceProvider();

        NullLoggerFactory loggerFactory = new();

        _backgroundMessageProcessor = new BackgroundMessageProcessor(messageChannel: _messageChannel,
            serviceProvider: serviceProvider, logger: loggerFactory.CreateLogger<BackgroundMessageProcessor>());

        await _backgroundMessageProcessor.StartAsync(cancellationToken: cancellationToken);
    }
}
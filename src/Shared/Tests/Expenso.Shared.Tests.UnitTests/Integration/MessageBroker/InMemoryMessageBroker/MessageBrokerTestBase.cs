using Expenso.Shared.Integration.Events;
using Expenso.Shared.Integration.MessageBroker;
using Expenso.Shared.Integration.MessageBroker.InMemory.Background;
using Expenso.Shared.Integration.MessageBroker.InMemory.Channels;
using Expenso.Shared.System.Logging;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging.Abstractions;

using Moq;

namespace Expenso.Shared.Tests.UnitTests.Integration.MessageBroker.InMemoryMessageBroker;

[TestFixture]
internal abstract class MessageBrokerTestBase : TestBase<IMessageBroker>
{
    [SetUp]
    public async Task SetUpAsync()
    {
        TestCandidate = new Shared.Integration.MessageBroker.InMemory.InMemoryMessageBroker(channel: _messageChannel);
        await StartMessageProcessor(cancellationToken: _stoppingTokenSource.Token);
    }

    [TearDown]
    public async Task TearDownAsync()
    {
        _messageChannel.Writer.Complete();
        await _backgroundMessageProcessor?.StopAsync(cancellationToken: _stoppingTokenSource.Token)!;
        _backgroundMessageProcessor?.Dispose();
    }

    private readonly Mock<ILoggerService<BackgroundMessageProcessor>> _loggerService = new();
    private readonly IMessageChannel _messageChannel = new MessageChannel();
    private readonly CancellationTokenSource _stoppingTokenSource = new();
    private BackgroundMessageProcessor? _backgroundMessageProcessor;

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
            serviceProvider: serviceProvider, logger: _loggerService.Object);

        await _backgroundMessageProcessor.StartAsync(cancellationToken: cancellationToken);
    }
}
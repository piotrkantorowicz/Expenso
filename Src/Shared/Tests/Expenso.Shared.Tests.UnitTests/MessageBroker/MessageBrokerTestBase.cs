using Expenso.Shared.IntegrationEvents;
using Expenso.Shared.MessageBroker;
using Expenso.Shared.MessageBroker.InMemory;
using Expenso.Shared.MessageBroker.InMemory.Background;
using Expenso.Shared.MessageBroker.InMemory.Channels;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace Expenso.Shared.Tests.UnitTests.MessageBroker;

internal abstract class MessageBrokerTestBase : TestBase
{
    private readonly IMessageChannel _messageChannel = new MessageChannel();
    private readonly CancellationTokenSource _stoppingTokenSource = new();
    private Task? _messageProcessorJob;

    protected IMessageBroker TestCandidate { get; private set; } = null!;

    [SetUp]
    public async Task SetUp()
    {
        TestCandidate = new InMemoryMessageBroker(_messageChannel);
        await StartMessageProcessor(_stoppingTokenSource.Token);
    }

    [TearDown]
    public async Task TearDown()
    {
        _messageChannel.Writer.Complete();
        await _stoppingTokenSource.CancelAsync();

        // Wait for the message processor to finish
        await Task.Delay(1000);
        _messageProcessorJob?.Dispose();
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

        NullLoggerFactory loggerFactory = new NullLoggerFactory();

        BackgroundMessageProcessor backgroundJob = new BackgroundMessageProcessor(_messageChannel, serviceProvider,
            loggerFactory.CreateLogger<BackgroundMessageProcessor>());

        _messageProcessorJob = backgroundJob.StartAsync(cancellationToken);
        await _messageProcessorJob;
    }
}
using System.Reflection;

using Expenso.Shared.Domain.Events;
using Expenso.Shared.Domain.Events.Dispatchers;
using Expenso.Shared.System.Configuration.Settings.App;
using Expenso.Shared.System.Logging;

using Microsoft.Extensions.DependencyInjection;

namespace Expenso.Shared.Tests.UnitTests.Domain.Events.DomainEventBroker;

[TestFixture]
internal abstract class DomainEventBrokerTestBase : TestBase<IDomainEventBroker>
{
    private IServiceProvider? _serviceProvider;

    [SetUp]
    public void SetUp()
    {
        Assembly[] assemblies = [typeof(DomainEventBrokerTestBase).Assembly];

        IServiceCollection serviceCollection = new ServiceCollection()
            .AddDomainEvents(assemblies: assemblies)
            .AddLogging()
            .AddInternalLogging();

        serviceCollection.AddSingleton<ApplicationSettings>();
        _serviceProvider = serviceCollection.BuildServiceProvider();
        TestCandidate = new Shared.Domain.Events.Dispatchers.DomainEventBroker(serviceProvider: _serviceProvider);
    }

    [TearDown]
    public void TearDown()
    {
        if (_serviceProvider is IDisposable disposable)
        {
            disposable.Dispose();
        }

        _serviceProvider = null;
        TestCandidate = null!;
    }
}
using System.Reflection;

using Expenso.Shared.Domain.Events;
using Expenso.Shared.Domain.Events.Dispatchers;
using Expenso.Shared.System.Configuration.Settings.App;
using Expenso.Shared.System.Logging;

using Microsoft.Extensions.DependencyInjection;

using TestCandidate = Expenso.Shared.Domain.Events.Dispatchers.DomainEventBroker;

namespace Expenso.Shared.Tests.UnitTests.Domain.Events.DomainEventBroker;

[TestFixture]
internal abstract class DomainEventBrokerTestBase : TestBase<IDomainEventBroker>
{
    [SetUp]
    public void SetUp()
    {
        Assembly[] assemblies = [typeof(DomainEventBrokerTestBase).Assembly];

        IServiceCollection serviceCollection = new ServiceCollection()
            .AddDomainEvents(assemblies: assemblies)
            .AddLogging()
            .AddInternalLogging();

        serviceCollection.AddSingleton<ApplicationSettings>();
        IServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();
        TestCandidate = new TestCandidate(serviceProvider: serviceProvider);
    }
}
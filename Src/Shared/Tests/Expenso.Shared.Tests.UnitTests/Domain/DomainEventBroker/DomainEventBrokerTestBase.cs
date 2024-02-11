using System.Reflection;

using Expenso.Shared.Domain.Events;
using Expenso.Shared.Domain.Events.Dispatchers;

using Microsoft.Extensions.DependencyInjection;

using TestCandidate = Expenso.Shared.Domain.Events.Dispatchers.DomainEventBroker;

namespace Expenso.Shared.Tests.UnitTests.Domain.DomainEventBroker;

internal abstract class DomainEventBrokerTestBase : TestBase<IDomainEventBroker>
{
    [SetUp]
    public void SetUp()
    {
        Assembly[] assemblies = [typeof(DomainEventBrokerTestBase).Assembly];

        ServiceProvider serviceProvider = new ServiceCollection()
            .AddDomainEvents(assemblies)
            .AddLogging()
            .BuildServiceProvider();

        TestCandidate = new TestCandidate(serviceProvider);
    }
}
using Expenso.Shared.Queries;
using Expenso.Shared.Queries.Dispatchers;

using Microsoft.Extensions.DependencyInjection;

namespace Expenso.Shared.Tests.UnitTests.Queries.QueryDispatchers;

internal abstract class QueryDispatcherTestBase : TestBase<IQueryDispatcher>
{
    [SetUp]
    public void Setup()
    {
        ServiceProvider serviceProvider = new ServiceCollection().AddQueries().BuildServiceProvider();
        TestCandidate = new QueryDispatcher(serviceProvider);
    }
}
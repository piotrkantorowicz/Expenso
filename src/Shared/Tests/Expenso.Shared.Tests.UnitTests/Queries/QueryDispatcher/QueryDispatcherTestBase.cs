using Expenso.Shared.Queries;
using Expenso.Shared.Queries.Dispatchers;

using Microsoft.Extensions.DependencyInjection;

namespace Expenso.Shared.Tests.UnitTests.Queries.QueryDispatcher;

[TestFixture]
internal abstract class QueryDispatcherTestBase : TestBase<IQueryDispatcher>
{
    [SetUp]
    public void Setup()
    {
        ServiceProvider serviceProvider = new ServiceCollection()
            .AddQueries(assemblies: [typeof(QueryDispatcherTestBase).Assembly])
            .BuildServiceProvider();

        TestCandidate = new Shared.Queries.Dispatchers.QueryDispatcher(serviceProvider: serviceProvider);
    }
}
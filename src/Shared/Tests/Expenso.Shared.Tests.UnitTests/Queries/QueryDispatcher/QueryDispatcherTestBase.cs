using Expenso.Shared.Queries;
using Expenso.Shared.Queries.Dispatchers;
using Expenso.Shared.Tests.Utils.UnitTests;

using Microsoft.Extensions.DependencyInjection;

using NUnit.Framework;

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

    [TearDown]
    public void TearDown()
    {
        TestCandidate = null!;
    }
}
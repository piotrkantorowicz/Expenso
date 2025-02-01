using Expenso.Shared.Commands.Dispatchers;
using Expenso.Shared.Queries.Dispatchers;
using Expenso.Shared.Tests.Utils.UnitTests;

using Moq;

using NUnit.Framework;

namespace Expenso.BudgetSharing.Tests.UnitTests.Application.Proxy.BudgetSharingProxy;

[TestFixture]
internal abstract class BudgetSharingProxyTestBase : TestBase<BudgetSharing.Application.Proxy.BudgetSharingProxy>
{
    [SetUp]
    public void SetUp()
    {
        TestCandidate = new BudgetSharing.Application.Proxy.BudgetSharingProxy(
            queryDispatcher: _queryDispatcherMock.Object, commandDispatcher: _commandDispatcherMock.Object,
            messageContextFactory: MessageContextFactoryMock.Object);
    }

    [TearDown]
    public void TearDown()
    {
        TestCandidate = null!;
    }

    protected readonly Mock<IQueryDispatcher> _queryDispatcherMock = new();
    protected readonly Mock<ICommandDispatcher> _commandDispatcherMock = new();
}
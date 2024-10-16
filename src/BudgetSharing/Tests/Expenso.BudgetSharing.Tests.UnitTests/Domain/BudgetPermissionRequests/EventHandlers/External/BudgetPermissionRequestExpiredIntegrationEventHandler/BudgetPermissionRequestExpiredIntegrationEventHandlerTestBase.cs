using Expenso.BudgetSharing.Domain.BudgetPermissionRequests.Services.Interfaces;
using Expenso.Shared.Tests.Utils.UnitTests;

using Moq;

using TestCandidate =
    Expenso.BudgetSharing.Domain.BudgetPermissionRequests.EventHandlers.External.BudgetPermissionRequestExpiredIntegrationEventHandler;

namespace Expenso.BudgetSharing.Tests.UnitTests.Domain.BudgetPermissionRequests.EventHandlers.External.
    BudgetPermissionRequestExpiredIntegrationEventHandler;

[TestFixture]
internal abstract class BudgetPermissionRequestExpiredIntegrationEventHandlerTestBase : TestBase<TestCandidate>
{
    [SetUp]
    public void SetUp()
    {
        _budgetPermissionRequestExpireDomainServiceMock = new Mock<IBudgetPermissionRequestExpirationDomainService>();

        TestCandidate =
            new TestCandidate(
                budgetPermissionRequestExpirationDomainService: _budgetPermissionRequestExpireDomainServiceMock.Object);
    }

    private Mock<IBudgetPermissionRequestExpirationDomainService> _budgetPermissionRequestExpireDomainServiceMock =
        null!;
}
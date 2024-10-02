using Expenso.BudgetSharing.Domain.BudgetPermissionRequests.Services.Interfaces;
using Expenso.Shared.Tests.Utils.UnitTests;

using Moq;

using TestCandidate =
    Expenso.BudgetSharing.Domain.BudgetPermissionRequests.EventHandlers.External.
    BudgetPermissionRequestExpiredIntegrationEventHandler;

namespace Expenso.BudgetSharing.Tests.UnitTests.Domain.BudgetPermissionRequests.EventHandlers.External.
    BudgetPermissionRequestExpiredIntegrationEventHandler;

internal abstract class BudgetPermissionRequestExpiredIntegrationEventHandlerTestBase : TestBase<TestCandidate>
{
    private Mock<IBudgetPermissionRequestExpirationDomainService> _budgetPermissionRequestExpireDomainServiceMock =
        null!;

    [SetUp]
    public void SetUp()
    {
        _budgetPermissionRequestExpireDomainServiceMock = new Mock<IBudgetPermissionRequestExpirationDomainService>();

        TestCandidate =
            new TestCandidate(
                budgetPermissionRequestExpirationDomainService: _budgetPermissionRequestExpireDomainServiceMock.Object);
    }
}
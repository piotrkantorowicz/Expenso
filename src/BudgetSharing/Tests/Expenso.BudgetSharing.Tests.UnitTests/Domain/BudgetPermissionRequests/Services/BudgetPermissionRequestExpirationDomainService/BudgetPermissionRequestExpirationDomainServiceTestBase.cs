using Expenso.BudgetSharing.Domain.BudgetPermissionRequests;
using Expenso.BudgetSharing.Domain.BudgetPermissionRequests.Repositories;
using Expenso.BudgetSharing.Domain.BudgetPermissionRequests.Services.Interfaces;
using Expenso.BudgetSharing.Domain.Shared.ValueObjects;
using Expenso.Shared.System.Types.Clock;

using Moq;

namespace Expenso.BudgetSharing.Tests.UnitTests.Domain.BudgetPermissionRequests.Services.
    BudgetPermissionRequestExpirationDomainService;

internal abstract class
    BudgetPermissionRequestExpirationDomainServiceTestBase : DomainTestBase<
    IBudgetPermissionRequestExpirationDomainService>
{
    protected BudgetPermissionRequest _budgetPermissionRequest = null!;
    protected Mock<IBudgetPermissionRequestRepository> _budgetPermissionRequestRepositoryMock = null!;
    private Mock<IClock> _clockMock = null!;

    [SetUp]
    public void Setup()
    {
        _budgetPermissionRequestRepositoryMock = new Mock<IBudgetPermissionRequestRepository>();
        _clockMock = new Mock<IClock>();

        _clockMock
            .Setup(expression: x => x.UtcNow)
            .Returns(value: new DateTimeOffset(year: 2024, month: 1, day: 1, hour: 0, minute: 0, second: 0,
                offset: TimeSpan.Zero));

        _budgetPermissionRequest = BudgetPermissionRequest.Create(budgetId: BudgetId.New(value: Guid.NewGuid()),
            personId: PersonId.New(value: Guid.NewGuid()), ownerId: PersonId.New(value: Guid.NewGuid()),
            permissionType: PermissionType.SubOwner, expirationDays: 3, clock: _clockMock.Object);

        TestCandidate =
            new BudgetSharing.Domain.BudgetPermissionRequests.Services.BudgetPermissionRequestExpirationDomainService(
                budgetPermissionRequestRepository: _budgetPermissionRequestRepositoryMock.Object);
    }
}
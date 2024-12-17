using Expenso.BudgetSharing.Domain.BudgetPermissionRequests;
using Expenso.BudgetSharing.Domain.BudgetPermissionRequests.Repositories;
using Expenso.BudgetSharing.Domain.BudgetPermissionRequests.Services.Interfaces;
using Expenso.BudgetSharing.Domain.BudgetPermissionRequests.ValueObjects;
using Expenso.BudgetSharing.Domain.Shared.ValueObjects;
using Expenso.Shared.System.Types.Clock;

using Moq;

namespace Expenso.BudgetSharing.Tests.UnitTests.Domain.BudgetPermissionRequests.Services.
    BudgetPermissionRequestExpirationDomainService;

[TestFixture]
internal abstract class
    BudgetPermissionRequestExpirationDomainServiceTestBase : DomainTestBase<
    IBudgetPermissionRequestExpirationDomainService>
{
    [SetUp]
    public void Setup()
    {
        _budgetPermissionRequestRepositoryMock = new Mock<IBudgetPermissionRequestRepository>();
        _clockMock = new Mock<IClock>();

        DateTimeOffset baseDate = new(year: 2024, month: 1, day: 1, hour: 0, minute: 0, second: 0,
            offset: TimeSpan.Zero);

        _clockMock.Setup(expression: x => x.UtcNow).Returns(value: baseDate);

        _budgetPermissionRequest = BudgetPermissionRequest.Create(
            budgetPermissionRequestId: BudgetPermissionRequestId.New(value: Guid.NewGuid()),
            budgetId: BudgetId.New(value: Guid.NewGuid()), personId: PersonId.New(value: Guid.NewGuid()),
            ownerId: PersonId.New(value: Guid.NewGuid()), budgetCode: BudgetCode.New(value: "BDGT/123/12/2024"),
            permissionType: PermissionType.SubOwner, expirationDate: baseDate.AddDays(days: DefaultExpirationDays),
            submissionDate: baseDate);

        TestCandidate =
            new BudgetSharing.Domain.BudgetPermissionRequests.Services.BudgetPermissionRequestExpirationDomainService(
                budgetPermissionRequestRepository: _budgetPermissionRequestRepositoryMock.Object);
    }

    [TearDown]
    public void TearDown()
    {
        _budgetPermissionRequestRepositoryMock.Reset();
        _clockMock.Reset();
        _budgetPermissionRequestRepositoryMock = null!;
        _budgetPermissionRequest = null!;
        _clockMock = null!;
        TestCandidate = null!;
    }

    private const int DefaultExpirationDays = 3;
    protected BudgetPermissionRequest _budgetPermissionRequest = null!;
    protected Mock<IBudgetPermissionRequestRepository> _budgetPermissionRequestRepositoryMock = null!;
    private Mock<IClock> _clockMock = null!;
}
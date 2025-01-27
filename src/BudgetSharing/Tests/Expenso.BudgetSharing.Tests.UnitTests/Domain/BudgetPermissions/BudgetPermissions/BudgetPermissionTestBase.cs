using Expenso.BudgetSharing.Domain.BudgetPermissions;
using Expenso.BudgetSharing.Domain.BudgetPermissions.Repositories;
using Expenso.BudgetSharing.Domain.BudgetPermissions.ValueObjects;
using Expenso.BudgetSharing.Domain.Shared.ValueObjects;
using Expenso.Shared.System.Time;

using Moq;

using NUnit.Framework;

namespace Expenso.BudgetSharing.Tests.UnitTests.Domain.BudgetPermissions.BudgetPermissions;

[TestFixture]
internal abstract class BudgetPermissionTestBase : DomainTestBase<BudgetPermission>
{
    [SetUp]
    public void SetUp()
    {
        _clockMock = new Mock<IClock>();
        _budgetPermissionRepositoryMock = new Mock<IBudgetPermissionRepository>();

        _budgetPermissionRepositoryMock
            .Setup(expression: x => x.IsUnique(_defaultBudgetPermissionId, _defaultBudgetId, _defaultOwnerId,
                _budgetCode, It.IsAny<CancellationToken>()))
            .ReturnsAsync(value: true);

        _clockMock
            .Setup(expression: x => x.UtcNow)
            .Returns(value: new DateTimeOffset(year: 2021, month: 1, day: 1, hour: 0, minute: 0, second: 0,
                offset: TimeSpan.Zero));
    }

    [TearDown]
    public void TearDown()
    {
        _clockMock.Reset();
        _budgetPermissionRepositoryMock.Reset();
        _clockMock = null!;
        _budgetPermissionRepositoryMock = null!;
    }

    protected readonly BudgetCode _budgetCode = BudgetCode.New(value: "BDGT/1234/5/2024");

    protected readonly BudgetId _defaultBudgetId =
        BudgetId.New(value: new Guid(g: "0194a81a-48c6-7ef7-995e-85772616b617"));

    protected readonly BudgetPermissionId _defaultBudgetPermissionId =
        BudgetPermissionId.New(value: new Guid(g: "0194a81a-48c6-7404-898d-535a0dc7f046"));

    protected readonly PersonId _defaultOwnerId =
        PersonId.New(value: new Guid(g: "0194a81a-48c6-7b0c-b54f-41136fd3c97f"));

    protected Mock<IBudgetPermissionRepository> _budgetPermissionRepositoryMock = null!;
    protected Mock<IClock> _clockMock = null!;

    protected BudgetPermission CreateTestCandidate(bool createDefaultPermission = true, bool emitDomainEvents = false)
    {
        BudgetPermission testCandidate = BudgetPermission.Create(budgetPermissionId: _defaultBudgetPermissionId,
            budgetId: _defaultBudgetId, ownerId: _defaultOwnerId, budgetCode: _budgetCode,
            budgetPermissionRepository: _budgetPermissionRepositoryMock.Object);

        if (createDefaultPermission)
        {
            testCandidate.AddPermission(participantId: _defaultOwnerId, permissionType: PermissionType.Owner);
        }

        if (!emitDomainEvents)
        {
            // Get uncommitted changes before assertions to clear domain events created during aggregate creation
            testCandidate.GetUncommittedChanges();
        }

        return testCandidate;
    }
}
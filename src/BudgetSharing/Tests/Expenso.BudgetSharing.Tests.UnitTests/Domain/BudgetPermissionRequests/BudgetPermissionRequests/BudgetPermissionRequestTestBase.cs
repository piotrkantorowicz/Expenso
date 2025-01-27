using Expenso.BudgetSharing.Domain.BudgetPermissionRequests;
using Expenso.BudgetSharing.Domain.BudgetPermissionRequests.ValueObjects;
using Expenso.BudgetSharing.Domain.Shared.ValueObjects;
using Expenso.Shared.System.Time;

using Moq;

using NUnit.Framework;

namespace Expenso.BudgetSharing.Tests.UnitTests.Domain.BudgetPermissionRequests.BudgetPermissionRequests;

[TestFixture]
internal abstract class BudgetPermissionRequestTestBase : DomainTestBase<BudgetPermissionRequest>
{
    [SetUp]
    public void SetUp()
    {
        _clockMock = new Mock<IClock>();
        _clockMock.Setup(expression: x => x.UtcNow).Returns(value: DateTimeOffset.UtcNow);

        _defaultBudgetPermissionId =
            BudgetPermissionRequestId.New(value: new Guid(g: "0194a81a-48c6-7e75-8610-f295c9c57996"));

        _defaultPersonId = PersonId.New(value: new Guid(g: "0194a81a-48c6-78fb-8e0c-35710bd1d287"));
        _defaultOwnerId = PersonId.New(value: new Guid(g: "0194a81a-48c6-779a-a1f9-e0b069274449"));
        _defaultBudgetId = BudgetId.New(value: new Guid(g: "0194a81a-48c6-7fd1-b19a-51a60562805d"));
        _budgetCode = BudgetCode.New(value: "BDGT/1004/12/2024");
        _defaultPermissionType = PermissionType.Reviewer;
    }

    [TearDown]
    public void TearDown()
    {
        _clockMock.Reset();
        _clockMock = null!;
        _defaultBudgetPermissionId = null!;
        _defaultPersonId = null!;
        _defaultOwnerId = null!;
        _defaultBudgetId = null!;
        _budgetCode = null!;
        _defaultPermissionType = null!;
        TestCandidate = null!;
    }

    protected const int Expiration = 3;
    protected Mock<IClock> _clockMock = new();
    protected BudgetPermissionRequestId _defaultBudgetPermissionId = null!;
    protected BudgetId _defaultBudgetId = null!;
    protected BudgetCode _budgetCode = null!;
    protected PersonId _defaultOwnerId = null!;
    protected PermissionType _defaultPermissionType = null!;
    protected PersonId _defaultPersonId = null!;

    protected BudgetPermissionRequest CreateTestCandidate(bool emitDomainEvents = false, int? delay = null)
    {
        // Set clock to 30 minutes ago to simulate creation of the aggregate in the past
        _clockMock
            .Setup(expression: x => x.UtcNow)
            .Returns(value: DateTimeOffset.UtcNow.AddMinutes(minutes: delay ?? -30));

        BudgetPermissionRequest testCandidate = BudgetPermissionRequest.Create(
            budgetPermissionRequestId: _defaultBudgetPermissionId, budgetId: _defaultBudgetId, ownerId: _defaultOwnerId,
            personId: _defaultPersonId, permissionType: _defaultPermissionType,
            expirationDate: _clockMock.Object.UtcNow.AddDays(days: Expiration),
            submissionDate: _clockMock.Object.UtcNow, budgetCode: _budgetCode);

        if (!emitDomainEvents)
        {
            // Get uncommitted changes before assertions to clear domain events created during aggregate creation
            testCandidate.GetUncommittedChanges();
        }

        _clockMock.Setup(expression: x => x.UtcNow).Returns(value: DateTimeOffset.UtcNow);

        return testCandidate;
    }
}
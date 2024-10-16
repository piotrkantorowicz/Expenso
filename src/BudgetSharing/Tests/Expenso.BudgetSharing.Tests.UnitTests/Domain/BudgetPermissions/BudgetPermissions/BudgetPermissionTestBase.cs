using Expenso.BudgetSharing.Domain.BudgetPermissions;
using Expenso.BudgetSharing.Domain.BudgetPermissions.ValueObjects;
using Expenso.BudgetSharing.Domain.Shared.ValueObjects;
using Expenso.Shared.System.Types.Clock;

using Moq;

namespace Expenso.BudgetSharing.Tests.UnitTests.Domain.BudgetPermissions.BudgetPermissions;

[TestFixture]
internal abstract class BudgetPermissionTestBase : DomainTestBase<BudgetPermission>
{
    [SetUp]
    public void SetUp()
    {
        _clockMock = new Mock<IClock>();

        _clockMock
            .Setup(expression: x => x.UtcNow)
            .Returns(value: new DateTimeOffset(year: 2021, month: 1, day: 1, hour: 0, minute: 0, second: 0,
                offset: TimeSpan.Zero));
    }

    protected readonly BudgetId _defaultBudgetId =
        BudgetId.New(value: new Guid(g: "c3e578f3-8ec1-4fbd-b680-64f9bbc77eba"));

    protected readonly BudgetPermissionId _defaultBudgetPermissionId =
        BudgetPermissionId.New(value: new Guid(g: "c3e578f3-8ec1-4fbd-b680-64f9bbc77eba"));

    protected readonly PersonId _defaultOwnerId =
        PersonId.New(value: new Guid(g: "c3e578f3-8ec1-4fbd-b680-64f9bbc77eba"));

    protected Mock<IClock> _clockMock = null!;

    protected BudgetPermission CreateTestCandidate(bool createDefaultPermission = true, bool emitDomainEvents = false)
    {
        BudgetPermission testCandidate = BudgetPermission.Create(budgetPermissionId: _defaultBudgetPermissionId,
            budgetId: _defaultBudgetId, ownerId: _defaultOwnerId);

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
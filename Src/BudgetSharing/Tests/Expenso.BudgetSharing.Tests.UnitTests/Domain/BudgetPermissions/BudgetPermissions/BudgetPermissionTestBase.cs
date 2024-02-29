using Expenso.BudgetSharing.Domain.BudgetPermissions;
using Expenso.BudgetSharing.Domain.BudgetPermissions.ValueObjects;
using Expenso.BudgetSharing.Domain.Shared.ValueObjects;
using Expenso.Shared.System.Types.Clock;

using Moq;

namespace Expenso.BudgetSharing.Tests.UnitTests.Domain.BudgetPermissions.BudgetPermissions;

internal abstract class BudgetPermissionTestBase : DomainTestBase<BudgetPermission>
{
    protected readonly BudgetPermissionId _defaultBudgetPermissionId =
        BudgetPermissionId.New(new Guid("c3e578f3-8ec1-4fbd-b680-64f9bbc77eba"));

    protected readonly BudgetId _defaultBudgetId = BudgetId.New(new Guid("c3e578f3-8ec1-4fbd-b680-64f9bbc77eba"));
    protected readonly PersonId _defaultPersonId = PersonId.New(new Guid("c3e578f3-8ec1-4fbd-b680-64f9bbc77eba"));
    protected Mock<IClock> _clockMock = null!;

    [SetUp]
    public void SetUp()
    {
        _clockMock = new Mock<IClock>();
        _clockMock.Setup(x => x.UtcNow).Returns(new DateTimeOffset(2021, 1, 1, 0, 0, 0, TimeSpan.Zero));
    }

    protected BudgetPermission CreateTestCandidate(bool createDefaultPermission = true, bool emitDomainEvents = false)
    {
        BudgetPermission testCandidate =
            BudgetPermission.Create(_defaultBudgetPermissionId, _defaultBudgetId, _defaultPersonId);

        if (createDefaultPermission)
        {
            testCandidate.AddPermission(_defaultPersonId, PermissionType.Owner);
        }

        if (!emitDomainEvents)
        {
            // Get uncommitted changes before assertions to clear domain events created during aggregate creation
            testCandidate.GetUncommittedChanges();
        }

        return testCandidate;
    }
}
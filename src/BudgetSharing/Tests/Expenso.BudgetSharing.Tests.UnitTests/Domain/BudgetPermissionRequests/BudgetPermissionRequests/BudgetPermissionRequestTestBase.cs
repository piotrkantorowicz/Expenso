using Expenso.BudgetSharing.Domain.BudgetPermissionRequests;
using Expenso.BudgetSharing.Domain.Shared.ValueObjects;
using Expenso.Shared.System.Types.Clock;

using Moq;

namespace Expenso.BudgetSharing.Tests.UnitTests.Domain.BudgetPermissionRequests.BudgetPermissionRequests;

internal abstract class BudgetPermissionRequestTestBase : DomainTestBase<BudgetPermissionRequest>
{
    protected const int Expiration = 3;
    protected readonly Mock<IClock> _clockMock = new();

    protected readonly BudgetId _defaultBudgetId =
        BudgetId.New(value: new Guid(g: "c3e578f3-8ec1-4fbd-b680-64f9bbc77eba"));

    protected readonly PersonId _defaultOwnerId =
        PersonId.New(value: new Guid(g: "fabfae93-2257-4bbc-ac90-8319d42c4836"));

    protected readonly PermissionType _defaultPermissionType = PermissionType.Reviewer;

    protected readonly PersonId _defaultPersonId =
        PersonId.New(value: new Guid(g: "c3e578f3-8ec1-4fbd-b680-64f9bbc77eba"));

    [SetUp]
    public void SetUp()
    {
    }

    protected BudgetPermissionRequest CreateTestCandidate(bool emitDomainEvents = false)
    {
        _clockMock.Setup(expression: x => x.UtcNow).Returns(value: DateTimeOffset.UtcNow);

        BudgetPermissionRequest testCandidate = BudgetPermissionRequest.Create(budgetId: _defaultBudgetId,
            ownerId: _defaultOwnerId, personId: _defaultPersonId, permissionType: _defaultPermissionType,
            expirationDays: Expiration, clock: _clockMock.Object);

        if (!emitDomainEvents)
        {
            // Get uncommitted changes before assertions to clear domain events created during aggregate creation
            testCandidate.GetUncommittedChanges();
        }

        return testCandidate;
    }
}
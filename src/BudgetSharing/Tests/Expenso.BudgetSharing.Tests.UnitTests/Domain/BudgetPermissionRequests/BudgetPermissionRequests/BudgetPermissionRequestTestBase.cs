using Expenso.BudgetSharing.Domain.BudgetPermissionRequests;
using Expenso.BudgetSharing.Domain.Shared.ValueObjects;
using Expenso.Shared.System.Types.Clock;

using Moq;

namespace Expenso.BudgetSharing.Tests.UnitTests.Domain.BudgetPermissionRequests.BudgetPermissionRequests;

[TestFixture]
internal abstract class BudgetPermissionRequestTestBase : DomainTestBase<BudgetPermissionRequest>
{
    [SetUp]
    public void SetUp()
    {
        _clockMock.Setup(expression: x => x.UtcNow).Returns(value: DateTimeOffset.UtcNow);
        _defaultPersonId = PersonId.New(value: new Guid(g: "be3220e9-54da-4013-a0dd-72db7ef3b53e"));
        _defaultOwnerId = PersonId.New(value: new Guid(g: "fabfae93-2257-4bbc-ac90-8319d42c4836"));
        _defaultBudgetId = BudgetId.New(value: new Guid(g: "c3e578f3-8ec1-4fbd-b680-64f9bbc77eba"));
        _defaultPermissionType = PermissionType.Reviewer;
    }

    protected const int Expiration = 3;
    protected readonly Mock<IClock> _clockMock = new();
    protected BudgetId _defaultBudgetId = null!;
    protected PersonId _defaultOwnerId = null!;
    protected PermissionType _defaultPermissionType = null!;
    protected PersonId _defaultPersonId = null!;

    protected BudgetPermissionRequest CreateTestCandidate(bool emitDomainEvents = false, int? delay = null)
    {
        // Set clock to 30 minutes ago to simulate creation of the aggregate in the past
        _clockMock
            .Setup(expression: x => x.UtcNow)
            .Returns(value: DateTimeOffset.UtcNow.AddMinutes(minutes: delay ?? -30));

        BudgetPermissionRequest testCandidate = BudgetPermissionRequest.Create(budgetId: _defaultBudgetId,
            ownerId: _defaultOwnerId, personId: _defaultPersonId, permissionType: _defaultPermissionType,
            expirationDate: _clockMock.Object.UtcNow.AddDays(days: Expiration),
            submissionDate: _clockMock.Object.UtcNow);

        if (!emitDomainEvents)
        {
            // Get uncommitted changes before assertions to clear domain events created during aggregate creation
            testCandidate.GetUncommittedChanges();
        }

        _clockMock.Setup(expression: x => x.UtcNow).Returns(value: DateTimeOffset.UtcNow);

        return testCandidate;
    }
}
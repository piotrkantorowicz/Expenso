using Expenso.BudgetSharing.Domain.BudgetPermissionRequests;
using Expenso.BudgetSharing.Domain.Shared.Model.ValueObjects;
using Expenso.Shared.Domain.Types.Events;
using Expenso.Shared.System.Types.Clock;
using Expenso.Shared.Tests.Utils.UnitTests;

using FluentAssertions;

using Moq;

namespace Expenso.BudgetSharing.Tests.UnitTests.Domain.BudgetPermissionRequests.BudgetPermissionRequests;

internal abstract class BudgetPermissionRequestTestBase : TestBase<BudgetPermissionRequest>
{
    protected readonly Mock<IClock> _clockMock = new();
    protected readonly BudgetId _defaultBudgetId = BudgetId.New(new Guid("c3e578f3-8ec1-4fbd-b680-64f9bbc77eba"));
    protected readonly PersonId _defaultPersonId = PersonId.New(new Guid("c3e578f3-8ec1-4fbd-b680-64f9bbc77eba"));
    protected readonly PermissionType _defaultPermissionType = PermissionType.Reviewer;
    protected const int Expiration = 3;

    [SetUp]
    public void SetUp()
    {
        MessageContextFactoryResolverInitializer.Initialize(MessageContextFactoryMock.Object);
    }

    protected BudgetPermissionRequest CreateTestCandidate(bool emitDomainEvents = false)
    {
        BudgetPermissionRequest testCandidate = BudgetPermissionRequest.Create(_defaultBudgetId, _defaultPersonId,
            _defaultPermissionType, Expiration, _clockMock.Object);

        if (!emitDomainEvents)
        {
            // Get uncommitted changes before assertions to clear domain events created during aggregate creation
            // ReSharper disable once UnusedVariable
            IReadOnlyCollection<IDomainEvent> domainEvents = testCandidate.GetUncommittedChanges();
        }

        return testCandidate;
    }

    protected void AssertDomainEventPublished(IEnumerable<IDomainEvent> domainEvents)
    {
        IReadOnlyCollection<IDomainEvent> existingDomainEvents = TestCandidate.GetUncommittedChanges();
        bool matchingDomainEvents = domainEvents.SequenceEqual(existingDomainEvents);
        matchingDomainEvents.Should().BeTrue();
    }
}
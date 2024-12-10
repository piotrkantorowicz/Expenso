using Expenso.BudgetSharing.Shared.DTO.MessageBus.BudgetPermissionRequests.ExpireAssigningParticipant;
using Expenso.TimeManagement.Core.Application.JobEntries.Shared.BackgroundJobs.Events;
using Expenso.TimeManagement.Core.Application.Shared.Settings;

using FluentAssertions;

namespace Expenso.TimeManagement.Tests.UnitTests.Application.Jobs.Shared.BackgroundJobs.Events.EventTypeResolver;

[TestFixture]
internal sealed class Resolve : EventTypeResolverTestBase
{
    [Test]
    public void Should_ResolveEventType_When_EventNameIsAllowed()
    {
        // Arrange
        const AllowedEventType eventName = AllowedEventType.BudgetPermissionRequestExpired;

        // Act
        Type result = TestCandidate.Resolve(eventName: eventName);

        // Assert
        result.Should().Be(expected: typeof(BudgetPermissionRequestExpiredIntegrationEvent));
    }

    [Test]
    public void Should_ThrowInvalidEventTypeException_When_EventNameIsNotAllowed()
    {
        // Arrange
        const AllowedEventType eventName = AllowedEventType.None;

        // Act
        Action act = () => TestCandidate.Resolve(eventName: eventName);

        // Assert
        act.Should().Throw<InvalidEventTypeException>();
    }

    [Test]
    public void Should_ThrowInvalidEventTypeException_When_EventNameIsNotDefined()
    {
        // Arrange
        const AllowedEventType eventName = (AllowedEventType)100;

        // Act
        Action act = () => TestCandidate.Resolve(eventName: eventName);

        // Assert
        act.Should().Throw<InvalidEventTypeException>();
    }
}
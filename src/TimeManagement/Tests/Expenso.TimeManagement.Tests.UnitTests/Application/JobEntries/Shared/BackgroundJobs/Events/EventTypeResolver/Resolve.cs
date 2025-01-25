using Expenso.BudgetSharing.Shared.DTO.MessageBus.BudgetPermissionRequests.ExpireAssigningParticipant;
using Expenso.TimeManagement.Core.Application.JobEntries.Shared.BackgroundJobs.Events;
using Expenso.TimeManagement.Core.Application.Shared.Settings;

using NUnit.Framework;

using Shouldly;

namespace Expenso.TimeManagement.Tests.UnitTests.Application.JobEntries.Shared.BackgroundJobs.Events.EventTypeResolver;

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
        result.ShouldBe(expected: typeof(BudgetPermissionRequestExpiredIntegrationEvent));
    }

    [Test]
    public void Should_ThrowInvalidEventTypeException_When_EventNameIsNotAllowed()
    {
        // Arrange
        const AllowedEventType eventName = AllowedEventType.None;

        // Act
        Action action = () => TestCandidate.Resolve(eventName: eventName);

        // Assert
        action.ShouldThrow<InvalidEventTypeException>();
    }

    [Test]
    public void Should_ThrowInvalidEventTypeException_When_EventNameIsNotDefined()
    {
        // Arrange
        const AllowedEventType eventName = (AllowedEventType)100;

        // Act
        Action action = () => TestCandidate.Resolve(eventName: eventName);

        // Assert
        action.ShouldThrow<InvalidEventTypeException>();
    }
}
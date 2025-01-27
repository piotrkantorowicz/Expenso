using Expenso.TimeManagement.Core.Application.Shared.Settings;

using NUnit.Framework;

using Shouldly;

namespace Expenso.TimeManagement.Tests.UnitTests.Application.JobEntries.Shared.BackgroundJobs.Events.EventTypeResolver;

[TestFixture]
internal sealed class IsAllowable : EventTypeResolverTestBase
{
    [Test]
    public void Should_ReturnTrue_When_EventNameIsAllowed()
    {
        // Arrange
        const AllowedEventType eventName = AllowedEventType.BudgetPermissionRequestExpired;

        // Act
        bool result = TestCandidate.IsAllowable(eventName: eventName);

        // Assert
        result.ShouldBeTrue();
    }

    [Test]
    public void Should_ReturnFalse_When_EventNameIsNotAllowed()
    {
        // Arrange
        const AllowedEventType eventName = AllowedEventType.None;

        // Act
        bool result = TestCandidate.IsAllowable(eventName: eventName);

        // Assert
        result.ShouldBeFalse();
    }

    [Test]
    public void Should_ReturnFalse_When_EventNameIsNotDefined()
    {
        // Arrange
        const AllowedEventType eventName = (AllowedEventType)100;

        // Act
        bool result = TestCandidate.IsAllowable(eventName: eventName);

        // Assert
        result.ShouldBeFalse();
    }
}
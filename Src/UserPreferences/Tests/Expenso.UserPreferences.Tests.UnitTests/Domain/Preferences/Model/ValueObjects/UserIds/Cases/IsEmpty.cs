using Expenso.UserPreferences.Core.Domain.Preferences.Model.ValueObjects;

namespace Expenso.UserPreferences.Tests.UnitTests.Domain.Preferences.Model.ValueObjects.UserIds.Cases;

internal sealed class IsEmpty : UserIdTestBase
{
    [Test]
    public void Should_ReturnsTrue_When_UserIdIsEmpty()
    {
        // Arrange
        TestCandidate = UserId.CreateDefault();

        // Act
        bool result = TestCandidate.IsEmpty();

        // Assert
        result.Should().BeTrue();
    }

    [Test]
    public void Should_ReturnsFalse_When_UserIdIsNotEmpty()
    {
        // Arrange
        TestCandidate = UserId.Create(Guid.NewGuid());

        // Act
        bool result = TestCandidate.IsEmpty();

        // Assert
        result.Should().BeFalse();
    }
}
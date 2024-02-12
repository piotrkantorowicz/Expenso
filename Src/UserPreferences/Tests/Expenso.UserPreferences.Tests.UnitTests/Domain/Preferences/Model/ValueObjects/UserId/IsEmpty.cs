namespace Expenso.UserPreferences.Tests.UnitTests.Domain.Preferences.Model.ValueObjects.UserId;

internal sealed class IsEmpty : UserIdTestBase
{
    [Test]
    public void Should_ReturnsTrue_When_UserIdIsEmpty()
    {
        // Arrange
        TestCandidate = Core.Domain.Preferences.Model.ValueObjects.UserId.Default();

        // Act
        bool result = TestCandidate.IsEmpty();

        // Assert
        result.Should().BeTrue();
    }

    [Test]
    public void Should_ReturnsFalse_When_UserIdIsNotEmpty()
    {
        // Arrange
        TestCandidate = Core.Domain.Preferences.Model.ValueObjects.UserId.New(Guid.NewGuid());

        // Act
        bool result = TestCandidate.IsEmpty();

        // Assert
        result.Should().BeFalse();
    }
}
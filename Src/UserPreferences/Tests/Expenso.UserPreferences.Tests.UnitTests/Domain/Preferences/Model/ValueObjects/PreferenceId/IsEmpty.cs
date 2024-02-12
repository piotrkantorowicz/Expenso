namespace Expenso.UserPreferences.Tests.UnitTests.Domain.Preferences.Model.ValueObjects.PreferenceId;

internal sealed class IsEmpty : PreferenceIdTestBase
{
    [Test]
    public void Should_ReturnsTrue_When_PreferenceIdIsEmpty()
    {
        // Arrange
        TestCandidate = Core.Domain.Preferences.Model.ValueObjects.PreferenceId.Default();

        // Act
        bool result = TestCandidate.IsEmpty();

        // Assert
        result.Should().BeTrue();
    }

    [Test]
    public void Should_ReturnsFalse_When_PreferenceIdIsNotEmpty()
    {
        // Arrange
        TestCandidate = Core.Domain.Preferences.Model.ValueObjects.PreferenceId.New(Guid.NewGuid());

        // Act
        bool result = TestCandidate.IsEmpty();

        // Assert
        result.Should().BeFalse();
    }
}
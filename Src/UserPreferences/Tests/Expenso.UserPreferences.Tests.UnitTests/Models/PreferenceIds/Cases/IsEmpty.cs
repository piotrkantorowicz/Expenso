using Expenso.UserPreferences.Core.Domain.Preferences.Model.ValueObjects;

namespace Expenso.UserPreferences.Tests.UnitTests.Models.PreferenceIds.Cases;

internal sealed class IsEmpty : PreferenceIdTestBase
{
    [Test]
    public void Should_ReturnsTrue_When_PreferenceIdIsEmpty()
    {
        // Arrange
        TestCandidate = PreferenceId.CreateDefault();

        // Act
        bool result = TestCandidate.IsEmpty();

        // Assert
        result.Should().BeTrue();
    }

    [Test]
    public void Should_ReturnsFalse_When_PreferenceIdIsNotEmpty()
    {
        // Arrange
        TestCandidate = PreferenceId.Create(Guid.NewGuid());

        // Act
        bool result = TestCandidate.IsEmpty();

        // Assert
        result.Should().BeFalse();
    }
}
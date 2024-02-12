using TestCandidate = Expenso.UserPreferences.Core.Domain.Preferences.Model.ValueObjects.PreferenceId;

namespace Expenso.UserPreferences.Tests.UnitTests.Domain.Preferences.Model.ValueObjects.PreferenceId;

internal sealed class New : PreferenceIdTestBase
{
    [Test]
    public void Should_CreatePreferenceIdWithCorrectGuidAsValue()
    {
        // Arrange
        // Act
        TestCandidate = TestCandidate.New(Guid.NewGuid());

        // Assert
        TestCandidate.Value.Should().NotBeEmpty();
    }
}
namespace Expenso.UserPreferences.Tests.UnitTests.Domain.Preferences.Model.ValueObjects.PreferenceId;

internal sealed class CreateDefault : PreferenceIdTestBase
{
    [Test]
    public void Should_CreatePreferenceIdWithEmptyGuidAsValue()
    {
        // Arrange
        // Act
        TestCandidate = Core.Domain.Preferences.Model.ValueObjects.PreferenceId.Default();

        // Assert
        TestCandidate.Value.Should().BeEmpty();
    }
}
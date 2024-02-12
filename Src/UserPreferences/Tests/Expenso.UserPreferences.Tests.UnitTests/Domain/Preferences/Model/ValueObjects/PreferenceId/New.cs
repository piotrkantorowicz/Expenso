namespace Expenso.UserPreferences.Tests.UnitTests.Domain.Preferences.Model.ValueObjects.PreferenceId;

internal sealed class New : PreferenceIdTestBase
{
    [Test]
    public void Should_CreatePreferenceIdWithCorrectGuidAsValue()
    {
        // Arrange
        // Act
        TestCandidate = Core.Domain.Preferences.Model.ValueObjects.PreferenceId.New(Guid.NewGuid());

        // Assert
        TestCandidate.Value.Should().NotBeEmpty();
    }
}
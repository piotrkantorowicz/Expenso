using Expenso.UserPreferences.Core.Domain.Preferences.Model.ValueObjects;

namespace Expenso.UserPreferences.Tests.UnitTests.Domain.Preferences.Model.ValueObjects.PreferenceIds.Cases;

internal sealed class CreateDefault : PreferenceIdTestBase
{
    [Test]
    public void Should_CreatePreferenceIdWithEmptyGuidAsValue()
    {
        // Arrange
        // Act
        TestCandidate = PreferenceId.CreateDefault();

        // Assert
        TestCandidate.Value.Should().BeEmpty();
    }
}
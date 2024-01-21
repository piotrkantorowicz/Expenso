using Expenso.UserPreferences.Core.Domain.Preferences.Model.ValueObjects;

namespace Expenso.UserPreferences.Tests.UnitTests.Domain.Preferences.Model.ValueObjects.PreferenceIds.Cases;

internal sealed class Create : PreferenceIdTestBase
{
    [Test]
    public void Should_CreatePreferenceIdWithCorrectGuidAsValue()
    {
        // Arrange
        // Act
        TestCandidate = PreferenceId.Create(Guid.NewGuid());

        // Assert
        TestCandidate.Value.Should().NotBeEmpty();
    }
}
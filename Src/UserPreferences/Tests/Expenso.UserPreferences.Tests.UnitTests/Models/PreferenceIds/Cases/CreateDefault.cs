using Expenso.UserPreferences.Core.Models;

namespace Expenso.UserPreferences.Tests.UnitTests.Models.PreferenceIds.Cases;

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
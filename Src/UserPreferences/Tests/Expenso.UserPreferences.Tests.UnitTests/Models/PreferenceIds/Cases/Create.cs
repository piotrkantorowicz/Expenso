using Expenso.UserPreferences.Core.Models;

namespace Expenso.UserPreferences.Tests.UnitTests.Models.PreferenceIds.Cases;

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
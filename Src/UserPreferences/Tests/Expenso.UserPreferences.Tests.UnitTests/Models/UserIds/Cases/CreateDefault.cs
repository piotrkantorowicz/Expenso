using Expenso.UserPreferences.Core.Domain.Preferences.Model.ValueObjects;

namespace Expenso.UserPreferences.Tests.UnitTests.Models.UserIds.Cases;

internal sealed class CreateDefault : UserIdTestBase
{
    [Test]
    public void Should_CreateUserIdWithEmptyGuidAsValue()
    {
        // Arrange
        // Act
        TestCandidate = UserId.CreateDefault();

        // Assert
        TestCandidate.Value.Should().BeEmpty();
    }
}
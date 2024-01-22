using Expenso.UserPreferences.Core.Domain.Preferences.Model.ValueObjects;

namespace Expenso.UserPreferences.Tests.UnitTests.Domain.Preferences.Model.ValueObjects.UserIds.Cases;

internal sealed class Create : UserIdTestBase
{
    [Test]
    public void Should_CreateUserIdWithCorrectGuidAsValue()
    {
        // Arrange
        // Act
        TestCandidate = UserId.Create(Guid.NewGuid());

        // Assert
        TestCandidate.Value.Should().NotBeEmpty();
    }
}
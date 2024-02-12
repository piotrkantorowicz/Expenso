using TestCandidate = Expenso.UserPreferences.Core.Domain.Preferences.Model.ValueObjects.UserId;

namespace Expenso.UserPreferences.Tests.UnitTests.Domain.Preferences.Model.ValueObjects.UserId;

internal sealed class New : UserIdTestBase
{
    [Test]
    public void Should_CreateUserIdWithCorrectGuidAsValue()
    {
        // Arrange
        // Act
        TestCandidate = TestCandidate.New(Guid.NewGuid());

        // Assert
        TestCandidate.Value.Should().NotBeEmpty();
    }
}
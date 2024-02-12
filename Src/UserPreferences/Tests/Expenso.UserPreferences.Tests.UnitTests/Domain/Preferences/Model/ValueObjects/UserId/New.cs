namespace Expenso.UserPreferences.Tests.UnitTests.Domain.Preferences.Model.ValueObjects.UserId;

internal sealed class New : UserIdTestBase
{
    [Test]
    public void Should_CreateUserIdWithCorrectGuidAsValue()
    {
        // Arrange
        // Act
        TestCandidate = Core.Domain.Preferences.Model.ValueObjects.UserId.New(Guid.NewGuid());

        // Assert
        TestCandidate.Value.Should().NotBeEmpty();
    }
}
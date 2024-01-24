namespace Expenso.UserPreferences.Tests.UnitTests.Domain.Preferences.Model.ValueObjects.UserId;

internal sealed class CreateDefault : UserIdTestBase
{
    [Test]
    public void Should_CreateUserIdWithEmptyGuidAsValue()
    {
        // Arrange
        // Act
        TestCandidate = Core.Domain.Preferences.Model.ValueObjects.UserId.CreateDefault();

        // Assert
        TestCandidate.Value.Should().BeEmpty();
    }
}
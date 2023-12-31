using Expenso.Shared.MessageBroker;
using Expenso.UserPreferences.Core.Models;

namespace Expenso.UserPreferences.Tests.UnitTests.Repositories.Cases;

internal sealed class UpdateAsync : PreferenceRepositoryTestBase
{
    private readonly Mock<IMessageBroker> _messageBrokerMock = new();

    [Test]
    public async Task Should_UpdatePreference_When_PreferenceExists()
    {
        // Arrange
        Preference preference = Preferences.First();

        preference.Update(GeneralPreference.Create(true), FinancePreference.Create(true, 5, true, 8),
            NotificationPreference.Create(true, 14), _messageBrokerMock.Object, default);

        // Act
        Preference updatedPreference = await TestCandidate.UpdateAsync(preference, default);

        // Assert
        updatedPreference.Should().NotBeNull();
        Preferences.Should().Contain(updatedPreference);
    }
}
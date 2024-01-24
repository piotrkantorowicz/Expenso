using Expenso.Shared.MessageBroker;
using Expenso.UserPreferences.Core.Domain.Preferences.Model;
using Expenso.UserPreferences.Core.Domain.Preferences.Model.ValueObjects;

namespace Expenso.UserPreferences.Tests.UnitTests.Persistence.Repositories.Cases;

internal sealed class UpdateAsync : PreferenceRepositoryTestBase
{
    private readonly Mock<IMessageBroker> _messageBrokerMock = new();

    [Test]
    public async Task Should_UpdatePreference_When_PreferenceExists()
    {
        // Arrange
        Preference preference = Preferences.First();

        preference.Update(GeneralPreference.Create(true), FinancePreference.Create(true, 5, true, 8),
            NotificationPreference.Create(true, 14));

        // Act
        Preference updatedPreference = await TestCandidate.UpdateAsync(preference, default);

        // Assert
        updatedPreference.Should().NotBeNull();
        Preferences.Should().Contain(updatedPreference);
    }
}
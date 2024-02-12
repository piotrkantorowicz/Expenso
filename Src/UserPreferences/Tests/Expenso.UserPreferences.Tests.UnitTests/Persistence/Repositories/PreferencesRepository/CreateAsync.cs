using Expenso.UserPreferences.Core.Domain.Preferences.Model;
using Expenso.UserPreferences.Core.Domain.Preferences.Model.ValueObjects;

namespace Expenso.UserPreferences.Tests.UnitTests.Persistence.Repositories.PreferencesRepository;

internal sealed class CreateAsync : PreferenceRepositoryTestBase
{
    [Test]
    public async Task Should_CreatePreference_When_PreferenceDoesNotExist()
    {
        // Arrange
        Preference preference = Preference.CreateDefault(PreferenceId.New(Guid.NewGuid()), UserId.New(Guid.NewGuid()));

        _preferenceDbSetMock
            .Setup(x => x.AddAsync(preference, It.IsAny<CancellationToken>()))
            .Callback<Preference, CancellationToken>((entity, _) => { AddPreference(entity); });

        // Act
        Preference createdPreference = await TestCandidate.CreateAsync(preference, default);

        // Assert
        createdPreference.Should().NotBeNull();
        Preferences.Should().Contain(createdPreference);
    }
}
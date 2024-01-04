using Expenso.UserPreferences.Core.Models;

namespace Expenso.UserPreferences.Tests.UnitTests.Repositories.Cases;

internal sealed class CreateAsync : PreferenceRepositoryTestBase
{
    [Test]
    public async Task Should_CreatePreference_When_PreferenceDoesNotExist()
    {
        // Arrange
        Preference preference = Preference.CreateDefault(Guid.NewGuid(), Guid.NewGuid());

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
using Expenso.UserPreferences.Core.Application.Preferences.Write.Commands.CreatePreference.Factories;
using Expenso.UserPreferences.Core.Domain.Preferences.Model;

namespace Expenso.UserPreferences.Tests.UnitTests.Persistence.EfCore.Repositories.PreferencesRepository;

internal sealed class CreateAsync : PreferenceRepositoryTestBase
{
    [Test]
    public async Task Should_CreatePreference_When_PreferenceDoesNotExist()
    {
        // Arrange
        Preference preference = PreferenceFactory.Create(Guid.NewGuid());

        _preferenceDbSetMock
            .Setup(x => x.AddAsync(preference, It.IsAny<CancellationToken>()))
            .Callback<Preference, CancellationToken>((entity, _) => { AddPreference(entity); });

        // Act
        Preference createdPreference = await TestCandidate.CreateAsync(preference, default);

        // Assert
        createdPreference.Should().NotBeNull();
        _preferenceDbSetMock.Object.Should().Contain(createdPreference);
    }
}
using Expenso.UserPreferences.Core.Application.Preferences.Write.Commands.CreatePreference.Factories;
using Expenso.UserPreferences.Core.Domain.Preferences.Model;

namespace Expenso.UserPreferences.Tests.UnitTests.Persistence.EfCore.Repositories.PreferencesRepository;

internal sealed class CreateAsync : PreferenceRepositoryTestBase
{
    [Test]
    public async Task Should_CreatePreference_When_PreferenceDoesNotExist()
    {
        // Arrange
        Preference preference = PreferenceFactory.Create(userId: Guid.NewGuid());

        _preferenceDbSetMock
            .Setup(expression: x => x.AddAsync(preference, It.IsAny<CancellationToken>()))
            .Callback<Preference, CancellationToken>(action: (entity, _) => { AddPreference(preference: entity); });

        // Act
        Preference createdPreference =
            await TestCandidate.CreateAsync(preference: preference, cancellationToken: default);

        // Assert
        createdPreference.Should().NotBeNull();
        _preferenceDbSetMock.Object.Should().Contain(expected: createdPreference);
    }
}
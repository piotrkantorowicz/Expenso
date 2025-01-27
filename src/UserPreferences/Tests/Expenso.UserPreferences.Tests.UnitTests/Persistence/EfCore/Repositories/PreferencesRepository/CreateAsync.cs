using Expenso.UserPreferences.Core.Application.Preferences.Write.Commands.CreatePreference.Factories;
using Expenso.UserPreferences.Core.Domain.Preferences.Model;

using Moq;

using NUnit.Framework;

using Shouldly;

namespace Expenso.UserPreferences.Tests.UnitTests.Persistence.EfCore.Repositories.PreferencesRepository;

[TestFixture]
internal sealed class CreateAsync : PreferenceRepositoryTestBase
{
    [Test]
    public async Task Should_CreatePreference_When_PreferenceDoesNotExist()
    {
        // Arrange
        Preference preference =
            PreferenceFactory.Create(preferenceId: Guid.CreateVersion7(), userId: Guid.CreateVersion7());

        _preferenceDbSetMock
            .Setup(expression: x => x.AddAsync(preference, It.IsAny<CancellationToken>()))
            .Callback<Preference, CancellationToken>(action: (entity, _) => AddPreference(preference: entity));

        // Act
        Preference createdPreference =
            await TestCandidate.CreateAsync(preference: preference, cancellationToken: default);

        // Assert
        createdPreference.ShouldNotBeNull();
        _preferenceDbSetMock.Object.ShouldContain(expected: createdPreference);
    }
}
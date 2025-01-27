using Expenso.UserPreferences.Core.Domain.Preferences.Model;
using Expenso.UserPreferences.Core.Domain.Preferences.Repositories.Specifications;

using NUnit.Framework;

using Shouldly;

namespace Expenso.UserPreferences.Tests.UnitTests.Persistence.EfCore.Repositories.PreferencesRepository;

[TestFixture]
internal sealed class GetByIdAsync : PreferenceRepositoryTestBase
{
    [Test, TestCaseSource(sourceName: nameof(_preferenceIds))]
    public async Task Should_ReturnPreference_When_PreferenceExists(Guid preferenceId)
    {
        // Arrange
        PreferenceQuerySpecification querySpecification = new()
        {
            PreferenceId = preferenceId,
            UseTracking = false
        };

        // Act
        Preference? preference = await TestCandidate.GetAsync(querySpecification: querySpecification,
            cancellationToken: default);

        // Assert
        preference.ShouldNotBeNull();
        preference.ShouldBe(expected: Preferences.Single(predicate: x => x.Id == preferenceId));
    }
}
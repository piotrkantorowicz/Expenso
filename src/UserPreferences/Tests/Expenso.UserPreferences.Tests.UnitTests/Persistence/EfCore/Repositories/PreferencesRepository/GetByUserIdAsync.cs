using Expenso.UserPreferences.Core.Domain.Preferences.Model;
using Expenso.UserPreferences.Core.Domain.Preferences.Repositories.Specifications;

using NUnit.Framework;

using Shouldly;

namespace Expenso.UserPreferences.Tests.UnitTests.Persistence.EfCore.Repositories.PreferencesRepository;

[TestFixture]
internal sealed class GetByUserIdAsync : PreferenceRepositoryTestBase
{
    [Test, TestCaseSource(sourceName: nameof(_userIds))]
    public async Task Should_ReturnPreference_When_PreferenceExists(Guid userId)
    {
        // Arrange
        PreferenceQuerySpecification querySpecification = new()
        {
            UserId = userId,
            UseTracking = false
        };

        // Act
        Preference? preference = await TestCandidate.GetAsync(querySpecification: querySpecification,
            cancellationToken: default);

        // Assert
        preference.ShouldNotBeNull();
        preference.ShouldBe(expected: _preferenceDbSetMock.Object.Single(predicate: x => x.UserId == userId));
    }
}